using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Apathy
{
    public class Client : Common
    {
        long socket = -1;
        Thread connectThread;

        // connecting state (volatile for thread safety)
        // ! USE LOCK !
        volatile bool _Connecting;
        public bool Connecting => _Connecting;

        // connected state (volatile for thread safety)
        // ! USE LOCK !
        volatile bool _Connected;
        public bool Connected => _Connected;

        // 'N' content buffers. this allows us to process messages without
        // allocations via ArraySegment
        protected byte[][] contentBuffers;

        // get next messages (can be more than one per tick)
        // -> pass queue so we don't need to create a new one each time!
        // IMPORTANT: NOT THREAD SAFE! otherwise header/content reading will be
        //            corrupted.
        bool lastConnected;
        int contentSize;
        public void GetNextMessages(Queue<Message> messages)
        {
            // only if Connected, otherwise there won't be any messages
            if (!Connected)
                return;

            // first of all: not connected before, but connected now?
            if (!lastConnected)
            {
                messages.Enqueue(new Message(0, EventType.Connected, new ArraySegment<byte>()));
                lastConnected = true;
            }
            // connected and detected a disconnect?
            else if (socket == -1 || WasDisconnected(socket))
            {
                // add disconnected message and close properly
                messages.Enqueue(new Message(0, EventType.Disconnected, new ArraySegment<byte>()));
                CleanUpAndClose();
            }
            // still connected? then read a few messages
            else
            {
                // IMPORTANT: read a few (not just one) messages per tick(!)
                // see Server.GetNextMessages for in-depth explanation!
                // (applies to client too. we don't want to use 100 frames to
                //  process 100 spawn messages)
                //
                // NOTE: we use 'contentBuffers.Length' instead of
                //       'MaxReceivesPerTickPerConnection' so that runtime
                //       changes of 'MaxReceivesPerTickPerConnection' won't
                //       cause NullReferenceExceptions. we only create the
                //       buffer once.
                int i;
                for (i = 0; i < contentBuffers.Length; ++i)
                {
                    // header not read yet? then read if available
                    if (contentSize == 0)
                    {
                        if (ReadIfAvailable(socket, 4, headerBuffer))
                        {
                            contentSize = Utils.BytesToIntBigEndian(headerBuffer);
                        }
                        // can't read content size yet. try again next frame.
                        else break;
                    }
                    // otherwise header was read last time. just read content.
                    // (don't break)

                    // try to read content
                    if (contentSize > 0)
                    {
                        // get a pointer to a content buffer. we read up
                        // to 'n' messages per tick, so use the n-th one
                        byte[] contentBuffer = contentBuffers[i];

                        // protect against allocation attacks. an attacker might send
                        // multiple fake '2GB header' packets in a row, causing the server
                        // to allocate multiple 2GB byte arrays and run out of memory.
                        if (contentSize <= MaxMessageSize)
                        {
                            // read it
                            if (ReadIfAvailable(socket, contentSize, contentBuffer))
                            {
                                // create ArraySegment from read content
                                ArraySegment<byte> segment = new ArraySegment<byte>(contentBuffer, 0, contentSize);
                                messages.Enqueue(new Message(0, EventType.Data, segment));

                                // reset contentSize for next time
                                contentSize = 0;
                            }
                            // can't fully read it yet. try again next frame.
                            else break;
                        }
                        else
                        {
                            Debug.LogWarning("[Client] possible allocation attack with a header of: " + contentSize + " bytes.");

                            // add disconnected message and close properly
                            // -> DO NOT wait for next GetNextMessage to
                            //    detect a closed client. otherwise Mirror
                            //    would think we are still connected until
                            //    next frame!
                            messages.Enqueue(new Message(0, EventType.Disconnected, new ArraySegment<byte>()));
                            CleanUpAndClose();

                            // no need to receive any more messages
                            break;
                        }
                    }
                    // no content yet. try again next frame.
                    else break;
                }

                // for debugging
                //if (i > 1) Debug.Log("[Client]: read multiple (" + i + ") messages this tick.");
            }
        }

        // the connect thread function
        void ConnectThreadFunction(NetworkEndPoint address)
        {
            // absolutely must wrap with try/catch, otherwise thread
            // exceptions are silent
            try
            {
                // connect (blocking)
                int error = 0;
                if (NativeBindings.network_connect(socket, ref address, ref error) == 0)
                {
                    // configure the socket
                    ConfigureSocket(socket);

                    // connect successful
                    // AFTER configuring the socket. so that we don't call recv
                    // while still blocking!
                    Debug.Log("[Client] connected!");
                    lock (this) { _Connected = true; }
                }
                // log errors if failed.
                // (ECONNABORTED is expected if we call Disconnect while connecting)
                else if ((NativeError)error != NativeError.ECONNABORTED)
                {
                    Debug.LogError("network_connect failed: " + (NativeError)error);
                }
            }
            catch (Exception exception)
            {
                // something went wrong. probably important.
                Debug.LogError("[Client] Connect Exception: " + exception);
            }
            finally
            {
                // we definitely aren't connecting anymore. either it worked or
                // it failed.
                lock (this) { _Connecting = false; }
            }
        }

        public void Connect(string hostname, ushort port)
        {
            // not if already started
            if (Connecting || Connected) return;

            // create 'N' content buffers
            contentBuffers = new byte[MaxReceivesPerTickPerConnection][];
            for (int i = 0; i < contentBuffers.Length; ++i)
            {
                // create content buffer depending on configured MaxMessageSize
                contentBuffers[i] = new byte[MaxMessageSize];
            }

            // reset state
            lastConnected = false;
            contentSize = 0;

            // resolve host name (if hostname. otherwise it returns the IP)
            // and connect to the first available address (IPv4 or IPv6)
            // => GetHostAddresses is BLOCKING (for a very short time). we could
            //    move it to the ConnectThread, but it's hardly worth the extra
            //    code since we would have to create the socket in ConnectThread
            //    too, which would require us to use locks around socket every-
            //    where. it's better to live with a <1s block (if any).
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(hostname);
                if (addresses.Length > 0)
                {
                    // try to parse the IP
                    string ip = addresses[0].ToString();
                    if (NetworkEndPoint.TryParse(ip, port, out NetworkEndPoint address))
                    {
                        // create the socket
                        int error = 0;
                        if (NativeBindings.network_create_socket(ref socket, ref address, ref error) == 0)
                        {
                            // note: no need to enable DualMode because we
                            // connect to IPv4 or IPv6 depending on 'hostname'
                            // and then we don't reuse the socket.

                            // we are connecting
                            _Connecting = true; // thread isn't running, no lock needed!
                            Debug.Log("[Client] connecting to: " + ip);

                            // connect is blocking. let's call it in the thread and
                            // return immediately.
                            connectThread = new Thread(() => { ConnectThreadFunction(address); });
                            connectThread.IsBackground = true;
                            connectThread.Start();
                        }
                        else Debug.LogError("network_create_socket failed: " + (NativeError)error);
                    }
                    else Debug.LogError("[Client] Connect: failed to parse ip: " + ip + " port: " + port);
                }
                // it's not an error. just an invalid host so log a warning.
                else Debug.LogWarning("[Client] Connect: failed to resolve host: " + hostname + " (no address found)");
            }
            catch (SocketException exception)
            {
                // it's not an error. just an invalid host so log a warning.
                Debug.LogWarning("[Client] Connect: failed to resolve host: " + hostname + " reason: " + exception);
            }
        }

        // clean up and close everything
        void CleanUpAndClose()
        {
            // close client if already created
            // (it's still -1 for a short moment after calling Connect)
            if (socket != -1)
            {
                int error = 0;
                if (NativeBindings.network_close(socket, ref error) != 0)
                    Debug.LogError("network_close client failed: " + (NativeError)error);
                socket = -1;
            }

            // not connected anymore
            lock (this)
            {
                _Connected = false;
                _Connecting = false;
            }

            // reset GetNextMessage helpers
            lastConnected = false;
            contentSize = 0;
        }

        public void Disconnect()
        {
            // only if connecting or connected
            if (Connecting || Connected)
            {
                // clean up and close the connection
                CleanUpAndClose();

                // let the connect thread finish gracefully. connect will abort
                // either way because we closed the socket.
                connectThread?.Join();
            }
        }

        // Send ArraySegment for allocation free calls. byte[] can be allocation
        // free too, but Mirror would require ArraySegment sending for that.
        public bool Send(ArraySegment<byte> segment)
        {
            // only if connected
            if (Connected)
            {
                // respect max message size to avoid allocation attacks.
                if (segment.Count <= MaxMessageSize)
                {
                    // try to send
                    if (!SendIfNotFull(socket, segment))
                    {
                        // didn't work, time to close the connection
                        // (aka TCP for games. instead of threads, we use huge
                        //  send buffers + nonblocking mode. and if buffer is
                        //  full, we consider it a timeout)
                        int error = 0;
                        if (NativeBindings.network_close(socket, ref error) != 0)
                            Debug.LogError("network_close client failed: " + (NativeError)error);
                        socket = -1;
                        return false;
                    }
                    return true;
                }
                Debug.LogError("[Client] Send: message too big: " + segment.Count + ". Limit: " + MaxMessageSize);
                return false;
            }
            Debug.LogWarning("[Client] Send: not connected!");
            return false;
        }

        // Send byte[] for ease of use. can be allocation free too if the same
        // byte[] is used by the caller.
        public bool Send(byte[] data) => Send(new ArraySegment<byte>(data));
    }
}