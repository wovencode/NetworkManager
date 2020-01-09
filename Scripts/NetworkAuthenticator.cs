// =======================================================================================
// NetworkAuthenticator
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace wovencode
{

    // ===================================================================================
	// NetworkAuthenticator
	// ===================================================================================
	[RequireComponent(typeof(wovencode.NetworkManager))]
    [DisallowMultipleComponent]
    public class NetworkAuthenticator : Mirror.NetworkAuthenticator
    {
    
        [Header("NetworkManager")]
		public wovencode.NetworkManager manager;
		
		[Header("Events")]
		public UnityEventString onRegisterEvent;
		public UnityEventString onLoginEvent;
		public UnityEventString onDeleteEvent;
		
		[Header("Message Texts")]
		public string msgLoginSuccess 		= ""; // no message here as it would display a popup on every login
		public string msgLoginFailure 		= "Login failed!";
		public string msgRegisterSuccess 	= "Registration successful!";
		public string msgRegisterFailure 	= "Registration failed!";
		public string msgDeleteSuccess 		= "Delete successful!";
		public string msgDeleteFailure 		= "Delete failed!";
		public string msgVersionMismatch	= "Client out of date!";
		
		[Header("Security")]
    	public string userNameSalt 		= "at_least_16_byte";
    	
    	[Header("Settings")]
		public bool checkApplicationVersion = true;
		
		[HideInInspector]public string userName 						= "";
        [HideInInspector]public string userPassword						= "";
		[HideInInspector]public byte accountAction 						= 0;
		
		[HideInInspector]public const byte NetworkActionRegisterLocal 	= 10;
		[HideInInspector]public const byte NetworkActionRegisterRemote 	= 11;
		[HideInInspector]public const byte NetworkActionLoginLocal 		= 20;
		[HideInInspector]public const byte NetworkActionLoginRemote 	= 21;
		[HideInInspector]public const byte NetworkActionDeleteLocal 	= 30;
		[HideInInspector]public const byte NetworkActionDeleteRemote 	= 31;
		
		// -------------------------------------------------------------------------------
		// GenerateHash
		// Helper function to generate a hash from the current userName, salt & account name
		// -------------------------------------------------------------------------------
		protected string GenerateHash()
		{
			return Tools.PBKDF2Hash(userPassword, userNameSalt + userName);
		}
		
		// -------------------------------------------------------------------------------
		// OnStartServer
		// @Server
		// -------------------------------------------------------------------------------
        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
            NetworkServer.RegisterHandler<LoginMessage>(OnLoginMessage, true);
        }
        
        // -------------------------------------------------------------------------------
        // OnStartClient
        // @Client
		// -------------------------------------------------------------------------------
        public override void OnStartClient()
        {
            NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
        }
        
        // -------------------------------------------------------------------------------
        // OnServerAuthenticate
        // @Server
		// -------------------------------------------------------------------------------
        public override void OnServerAuthenticate(NetworkConnection conn)
        {
            // do nothing...wait for AuthRequestMessage from client
        }
        
        // -------------------------------------------------------------------------------
        // OnClientAuthenticate
        // @Client
		// -------------------------------------------------------------------------------
        public override void OnClientAuthenticate(NetworkConnection conn)
        {
        
            AuthRequestMessage authRequestMessage = new AuthRequestMessage
            {
                authUsername 	= userName,
                authPassword 	= GenerateHash(),
                authAction 		= accountAction,
                authVersion		= Application.version
            };

            NetworkClient.Send(authRequestMessage);
        }
        
        // -------------------------------------------------------------------------------
        // OnAuthRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------
        public void OnAuthRequestMessage(NetworkConnection conn, AuthRequestMessage msg)
        {
        	
			AuthResponseMessage authResponseMessage = new AuthResponseMessage
			{
				code 				= 100,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			// ------ Check version
			if (checkApplicationVersion && msg.authVersion != Application.version)
			{
				authResponseMessage.text = msgVersionMismatch;
            	authResponseMessage.code++;
			}
			else
			{
			
				// ------ Login to existing Account
				if (msg.authAction == NetworkActionLoginLocal || msg.authAction == NetworkActionLoginRemote)
				{
					if (Database.singleton.TryLogin(msg.authUsername, msg.authPassword))
					{
						authResponseMessage.text = msgLoginSuccess;
						onLoginEvent.Invoke(msg.authUsername);
					}
					else
					{
						authResponseMessage.text = msgLoginFailure;
						authResponseMessage.code++;
					}
				
					authResponseMessage.causesDisconnect = false; // does not cause disconnect
				
				}
			
				// ------ Register new Account
				if (msg.authAction == NetworkActionRegisterLocal || msg.authAction == NetworkActionRegisterRemote)
				{
					if (Database.singleton.TryRegister(msg.authUsername, msg.authPassword))
					{
						authResponseMessage.text = msgRegisterSuccess;
						onRegisterEvent.Invoke(msg.authUsername);
					}
					else
					{
						authResponseMessage.text = msgRegisterFailure;
						authResponseMessage.code++;
					}
				
					authResponseMessage.causesDisconnect = true; // causes disconnect
				}
			
				// ------ Delete existing Account
				if (msg.authAction == NetworkActionDeleteLocal || msg.authAction == NetworkActionDeleteRemote)
				{
					if (Database.singleton.TryDelete(msg.authUsername, msg.authPassword))
					{
						authResponseMessage.text = msgDeleteSuccess;
						onDeleteEvent.Invoke(msg.authUsername);
					}
					else
					{
						authResponseMessage.text = msgDeleteFailure;
						authResponseMessage.code++;
					}
				
					authResponseMessage.causesDisconnect = true; // causes disconnect
				}
			
				/*
					...
					add more cases here (like confirm account, change password etc.)
					...
				*/
			
            }
            
            // ------ Authenticate & Response Message
            if (authResponseMessage.code == 100)
            	base.OnServerAuthenticated.Invoke(conn);
            
			conn.Send(authResponseMessage);
			
			if (authResponseMessage.code > 100)
			{
				conn.isAuthenticated = false;
				conn.Disconnect();
			}
			
        }
		
		// -------------------------------------------------------------------------------
		// OnAuthResponseMessage
		// @Server -> @Client
		// -------------------------------------------------------------------------------
        public void OnAuthResponseMessage(NetworkConnection conn, AuthResponseMessage msg)
        {
        
        	if (!String.IsNullOrWhiteSpace(msg.text))
               	UIPopupConfirm.singleton.Init(msg.text);
            
            if (msg.code == 100 && !msg.causesDisconnect)
            {
               	base.OnClientAuthenticated.Invoke(conn);
               	ClientScene.Ready(conn);
               	conn.Send(new LoginMessage{ authUsername = userName, authPassword = GenerateHash() });
            }
            
            if (msg.code != 100 || msg.causesDisconnect)
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                manager.StopClient();
            }
        }
        
        // -------------------------------------------------------------------------------
        // OnLoginMessage
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        public void OnLoginMessage(NetworkConnection conn, LoginMessage msg)
        {
        	if (Database.singleton.TryLogin(msg.authUsername, msg.authPassword))
        		manager.LoginPlayer(conn, msg.authUsername);
        }
        
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================