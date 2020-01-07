#include "network.bindings.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#if PLATFORM_WIN
#include <windows.h>
#include <malloc.h>
#include <Mstcpip.h>
#else
#include <unistd.h>
#include <fcntl.h>
#include <sys/ioctl.h>
#include <sys/socket.h>
#include <sys/time.h>
#include <poll.h>
#define closesocket(x) \
    close(x)
#endif

static int g_initialized;

static int32_t
native_get_last_error()
{
#if PLATFORM_WIN
    return WSAGetLastError();
#else
    return errno;
#endif
}

// initialize & terminate //////////////////////////////////////////////////////
EXPORT_API int32_t
network_initialize()
{
    // only if not initialized yet
    if (g_initialized == 0)
    {
#if PLATFORM_WIN
        WSADATA wsaData = {0};
        WSAStartup(MAKEWORD(2, 2), &wsaData);
#endif
        g_initialized = 1;
    }
    return 0;
}

EXPORT_API int32_t
network_terminate()
{
    // only if was initialized before
    if (g_initialized == 1)
    {
#if PLATFORM_WIN
        // "In a multithreaded environment, WSACleanup terminates Windows
        // Sockets operations for all threads."
        // https://docs.microsoft.com/en-us/windows/win32/api/winsock/nf-winsock-wsacleanup
        //
        // it's not a good idea to call this in Unity. in fact, if we call it
        // twice in a row then there is a chance of Unity crashing in Windows.
        // -> Unity probably calls this internally when closing anyway. It uses
        //    sockets after all.
        //
        //WSACleanup();
#endif
        // we didn't call WSACleanup, so let's not reset this flag either.
        // otherwise calling network_initialize again would call WSAStartup again
        //g_initialized = 0;
    }
    return 0;
}

// create & close //////////////////////////////////////////////////////////////
EXPORT_API int32_t
network_create_socket(int64_t* sock, network_address* address, int32_t* error)
{
    int enable = 1;
    int64_t s = socket(address->addr.sa_family, SOCK_STREAM, 0);
#if PLATFORM_WIN
    // windows invalid socket check:
    // https://docs.microsoft.com/en-us/windows/win32/winsock/socket-data-type-2
    if (s == INVALID_SOCKET)
#else
    if (s == -1)
#endif
    {
        *error = native_get_last_error();
        return -1;
    }

    // make immediately reusable after calling close()
    // otherwise we get EADDRINUSE after pressing Play again in Unity!
#if PLATFORM_WIN
    if (setsockopt(s, SOL_SOCKET, SO_REUSEADDR, (char*)&enable, sizeof(int)) != 0)
#else
    if (setsockopt(s, SOL_SOCKET, SO_REUSEADDR, &enable, sizeof(int)) != 0)
#endif
    {
        closesocket(s);
        return -1;
    }

    *sock = s;
    return 0;
}

EXPORT_API int32_t
network_close(int64_t sock, int32_t* error)
{
    if (closesocket(sock) != 0)
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

// configuration ///////////////////////////////////////////////////////////////
EXPORT_API int32_t
network_set_dualmode(int64_t sock, int enabled, int32_t* error)
{
    // IPV6_ONLY requires an 'off' flag
    int off = 1;
    if (enabled)
        off = 0;

    if (setsockopt(sock, IPPROTO_IPV6, IPV6_V6ONLY, (void*)&off, sizeof(off)) != 0)
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

EXPORT_API int32_t
network_set_nonblocking(int64_t sock, int32_t* error)
{
#if PLATFORM_WIN
    DWORD arg = 1;
    // on success, 0 is returned
    if (ioctlsocket(sock, FIONBIO, &arg) != 0)
#else
    int flags = fcntl(sock, F_GETFL, 0);
    // on error, -1 is returned
    if (fcntl(sock, F_SETFL, flags | O_NONBLOCK) == -1)
#endif
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

EXPORT_API int32_t
network_set_send_buffer_size(int64_t sock, int size, int32_t* error)
{
    // note: on linux, the kernel doubles this value (to allow space for
    // bookkeeping overhead)
    // see also: https://linux.die.net/man/7/socket (SO_SNDBUF)
    if (setsockopt(sock, SOL_SOCKET, SO_SNDBUF, (void*)&size, sizeof(size)) != 0)
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

EXPORT_API int32_t
network_set_receive_buffer_size(int64_t sock, int size, int32_t* error)
{
    // note: on linux, the kernel doubles this value (to allow space for
    // bookkeeping overhead)
    // see also: https://linux.die.net/man/7/socket (SO_RCVBUF)
    if (setsockopt(sock, SOL_SOCKET, SO_RCVBUF, (void*)&size, sizeof(size)) != 0)
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

EXPORT_API int32_t
network_set_nodelay(int64_t sock, int enabled, int32_t* error)
{
    if (setsockopt(sock, IPPROTO_TCP, TCP_NODELAY, (void*)&enabled, sizeof(enabled)) != 0)
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

EXPORT_API int32_t
network_set_keepalive(int64_t sock, int enabled, int32_t* error)
{
    if (setsockopt(sock, SOL_SOCKET, SO_KEEPALIVE, (void*)&enabled, sizeof(enabled)) != 0)
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

EXPORT_API int32_t
network_get_send_buffer_size(int64_t sock, int* size, int32_t* error)
{
    // note: on linux, the kernel doubles the value that we pass in setsockopt
    // (to allow space for bookkeeping overhead)
    // see also: https://linux.die.net/man/7/socket (SO_SNDBUF)
    socklen_t len = sizeof(int);
    if (getsockopt(sock, SOL_SOCKET, SO_SNDBUF, (void*)size, &len) != 0)
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

EXPORT_API int32_t
network_get_receive_buffer_size(int64_t sock, int* size, int32_t* error)
{
    // note: on linux, the kernel doubles the value that we pass in setsockopt
    // (to allow space for bookkeeping overhead)
    // see also: https://linux.die.net/man/7/socket (SO_RCVBUF)
    socklen_t len = sizeof(int);
    if (getsockopt(sock, SOL_SOCKET, SO_RCVBUF, (void*)size, &len) != 0)
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

// check last error, can be used as 'is_connected' check
EXPORT_API int32_t
network_get_error(int64_t sock)
{
    int error = 0;
    socklen_t len = sizeof(error);
    if (getsockopt(sock, SOL_SOCKET, SO_ERROR, (void*)&error, &len) != 0)
    {
        // there was a problem getting the error code
        return -1;
    }
    // if error is 0 then everything is fine
    return error;
}

// client //////////////////////////////////////////////////////////////////////
EXPORT_API int32_t
network_connect(int64_t sock, network_address* address, int32_t* error)
{
#if PLATFORM_WIN
    if (connect(sock, (SOCKADDR*)address, (int)address->length) != 0)
#else
    if (connect(sock, (struct sockaddr*)address, (int)address->length) != 0)
#endif
    {
        *error = native_get_last_error();
        closesocket(sock);
        return -1;
    }
    return 0;
}

// server //////////////////////////////////////////////////////////////////////
EXPORT_API int32_t
network_bind(int64_t sock, network_address* address, int32_t* error)
{
#if PLATFORM_WIN
    if (bind(sock, (SOCKADDR*)address, (int)address->length) != 0)
#else
    if (bind(sock, (struct sockaddr*)address, (int)address->length) != 0)
#endif
    {
        *error = native_get_last_error();
        closesocket(sock);
        return -1;
    }
    return 0;
}

EXPORT_API int32_t
network_listen(int64_t sock, int32_t* error)
{
    if (listen(sock, 128) != 0)
    {
        *error = native_get_last_error();
        closesocket(sock);
        return -1;
    }

    return 0;
}

EXPORT_API int32_t
network_accept(int64_t sock, int64_t* client_sock, network_address* client_address, int32_t* error)
{
    int64_t c = 0;
    socklen_t client_len = sizeof(*client_address);
#if PLATFORM_WIN
    c = accept(sock, (SOCKADDR*) client_address, &client_len);
#else
    c = accept(sock, (struct sockaddr*) client_address, &client_len);
#endif
    if (c < 0)
    {
        *error = native_get_last_error();
        return -1;
    }

    *client_sock = c;
    return 0;
}

EXPORT_API int32_t
network_get_peer_address(int64_t sock, network_address* address, int32_t* error)
{
#if PLATFORM_WIN
    if (getpeername(sock, (struct sockaddr*)address, (int*)&address->length) != 0)
#else
    if (getpeername(sock, (struct sockaddr*)address, (socklen_t*)&address->length) != 0)
#endif
    {
        *error = native_get_last_error();
        return -1;
    }
    return 0;
}

// state ///////////////////////////////////////////////////////////////////////
EXPORT_API int32_t
network_disconnected(int64_t sock)
{
    // a good way to detect tcp disconnects is to check poll for receives, then
    // call recv with MSG_PEEK and a size>0 to see if it would work or not,
    // without actually touching the stream (because of MSG_PEEK)
    //
    // source:
    //   http://stefan.buettcher.org/cs/conn_closed.html
    //
    // note:
    //   the article uses poll, but WSAPoll is broken on Windows:
    //   https://daniel.haxx.se/blog/2012/10/10/wsapoll-is-broken/
    //   so we use select instead.
    fd_set fdread;
    struct timeval tv;
    char buffer[1];

    // initialize fd
    FD_ZERO(&fdread);
    FD_SET(sock, &fdread);

    // set timeout to return immediately
    tv.tv_sec = 0;
    tv.tv_usec = 0;

    // call select to check for socket activity
    // note: nfds parameter needs an int cast to avoid a warning on windows.
    //       this is fine, because on linux, all the sockets are int32 anyway,
    //       and on windows the first parameter of select is ignored.
    if (select((int)sock + 1, &fdread, NULL, NULL, &tv) > 0)
    {
        // if result > 0, this means that there is either data available on the
        // socket, or the socket has been closed. PEEK to see if reading would
        // fail
#if PLATFORM_WIN
        // note: windows doesn't have MSG_DONTWAIT, so this only works if in
        //       non-blocking mode
        if (recv(sock, buffer, sizeof(buffer), MSG_PEEK) <= 0)
#else
        if (recv(sock, buffer, sizeof(buffer), MSG_PEEK | MSG_DONTWAIT) <= 0)
#endif
        {
            // if recv returns zero, that means the connection has been closed
            return 1;
        }
    }
    return 0;
}

// recv & send /////////////////////////////////////////////////////////////////
// read how many bytes are available before calling recv
EXPORT_API int32_t
network_available(int64_t sock, int32_t* error)
{
    int available = 0;

#if PLATFORM_WIN
    if (ioctlsocket(sock, FIONREAD, &available) != 0)
#else
    if (ioctl(sock, FIONREAD, &available) != 0)
#endif
    {
        *error = native_get_last_error();
        closesocket(sock);
        return -1;
    }

    return available;
}

EXPORT_API int32_t
network_recv(int64_t sock, void* buffer, int32_t len, int32_t* error)
{
    int read = recv(sock, buffer, len, 0);
    if (read < 0)
    {
        *error = native_get_last_error();
        closesocket(sock);
        return -1;
    }
    return read;
}

EXPORT_API int32_t
network_send(int64_t sock, void* buffer, int32_t len, int32_t* error)
{
    int sent = send(sock, buffer, len, 0);
    if (sent < 0)
    {
        *error = native_get_last_error();
        closesocket(sock);
        return -1;
    }
    return sent;
}