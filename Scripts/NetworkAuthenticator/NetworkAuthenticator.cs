// =======================================================================================
// Wovencore
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
    public partial class NetworkAuthenticator : BaseNetworkAuthenticator
    {
    
        [Header("NetworkManager")]
		public wovencode.NetworkManager manager;
		
		[Header("Events")]
		public UnityEventString onRegisterEvent;
		public UnityEventString onLoginEvent;
		public UnityEventString onDeleteEvent;
		public UnityEventString onConfirmEvent;
		public UnityEventString onSwitchServerEvent;
		
		[Header("Message Texts")]
		public string msgLoginSuccess 			= ""; // no message here as it would display a popup on every login
		public string msgLoginFailure 			= "Login failed!";
		public string msgRegisterSuccess 		= "Registration successful!";
		public string msgRegisterFailure 		= "Registration failed!";
		public string msgDeleteSuccess 			= "Delete successful!";
		public string msgDeleteFailure 			= "Delete failed!";
		public string msgConfirmSuccess 		= "Account confirmed!";
		public string msgConfirmFailure 		= "Confirm failed!";
		public string msgSwitchServerSuccess 	= "Server switch successful!";
		public string msgSwitchServerFailure 	= "Server switch failed!";
		public string msgVersionMismatch		= "Client out of date!";
		
		[Header("Security")]
    	public string userNameSalt 				= "at_least_16_byte";
    	
    	[Header("Settings")]
		public bool checkApplicationVersion 	= true;
		
		[HideInInspector]public string userName 						= "";
        [HideInInspector]public string userPassword						= "";
		[HideInInspector]public byte accountAction 						= 0;
		
		[HideInInspector]public const byte NetworkActionRegister 		= 10;
		[HideInInspector]public const byte NetworkActionLogin 			= 20;
		[HideInInspector]public const byte NetworkActionDelete 			= 30;
		[HideInInspector]public const byte NetworkActionConfirm 		= 40;
		[HideInInspector]public const byte NetworkActionSwitchServer 	= 50;
		
		protected const byte successCode 	= 100;
		protected const byte errorCode 		= 200;
		
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
            NetworkServer.RegisterHandler<LoginRequestMessage>(OnLoginRequestMessage, true);
            NetworkServer.RegisterHandler<RegisterRequestMessage>(OnRegisterRequestMessage, true);
            NetworkServer.RegisterHandler<DeleteRequestMessage>(OnDeleteRequestMessage, true);
            NetworkServer.RegisterHandler<ConfirmRequestMessage>(OnConfirmRequestMessage, true);
            NetworkServer.RegisterHandler<SwitchServerRequestMessage>(OnSwitchServerRequestMessage, true);
        }
        
        // -------------------------------------------------------------------------------
        // OnStartClient
        // @Client
		// -------------------------------------------------------------------------------
        public override void OnStartClient()
        {
            NetworkClient.RegisterHandler<ServerResponseMessage>(OnServerResponseMessage, false);
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
        // @Client -> @Server
		// -------------------------------------------------------------------------------
        public override void OnClientAuthenticate(NetworkConnection conn)
        {
            AuthRequestMessage authRequestMessage = new AuthRequestMessage
            {
                clientVersion = Application.version
            };

            NetworkClient.Send(authRequestMessage);
        }
        
        // -------------------------------------------------------------------------------
        // OnAuthRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------      
        void OnAuthRequestMessage(NetworkConnection conn, AuthRequestMessage msg)
		{
		
			ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (checkApplicationVersion && msg.clientVersion != Application.version)
			{
				message.text = msgVersionMismatch;
            	message.code = errorCode;
			}
			else
			{
				base.OnServerAuthenticated.Invoke(conn);
			}
			
			conn.Send(message);
			
			if (message.code == errorCode)
			{
				conn.isAuthenticated = false;
				conn.Disconnect();
			}
		
		}
        
        // -------------------------------------------------------------------------------
        // OnLoginRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------      
        void OnLoginRequestMessage(NetworkConnection conn, LoginRequestMessage msg)
		{
			
			ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryLogin(msg.username, msg.password))
			{
				manager.LoginPlayer(conn, msg.username);
				message.text = msgLoginSuccess;
				onLoginEvent.Invoke(msg.username);
			}
			else
			{
				message.text = msgLoginFailure;
				message.code = errorCode;
			}
					
			conn.Send(message);
			
		}
		
       	// -------------------------------------------------------------------------------
        // OnRegisterRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnRegisterRequestMessage(NetworkConnection conn, RegisterRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= true
			};
        	
        	if (DatabaseManager.singleton.TryRegister(msg.username, msg.password))
			{
				message.text = msgRegisterSuccess;
				onRegisterEvent.Invoke(msg.username);
			}
			else
			{
				message.text = msgRegisterFailure;
				message.code = errorCode;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnDeleteRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnDeleteRequestMessage(NetworkConnection conn, DeleteRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TrySoftDelete(msg.username, msg.password))
			{
				message.text = msgDeleteSuccess;
				onDeleteEvent.Invoke(msg.username);
			}
			else
			{
				message.text = msgDeleteFailure;
				message.code = errorCode;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnConfirmRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnConfirmRequestMessage(NetworkConnection conn, ConfirmRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryConfirm(msg.username, msg.password))
			{
				message.text = msgConfirmSuccess;
				onConfirmEvent.Invoke(msg.username);
			}
			else
			{
				message.text = msgConfirmFailure;
				message.code = errorCode;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnSwitchServerRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
       	void OnSwitchServerRequestMessage(NetworkConnection conn, SwitchServerRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TrySwitchServer(msg.username, msg.token))
			{
				message.text = msgSwitchServerSuccess;
				onSwitchServerEvent.Invoke(msg.username);
			}
			else
			{
				message.text = msgSwitchServerFailure;
				message.code = errorCode;
			}
			
        	conn.Send(message);
        	
        }
        
		// -------------------------------------------------------------------------------
		// OnServerResponseMessage
		// @Server -> @Client
		// -------------------------------------------------------------------------------
        void OnServerResponseMessage(NetworkConnection conn, ServerResponseMessage msg)
        {
        	
        	// -- show popup if error message is not empty
        	if (!String.IsNullOrWhiteSpace(msg.text))
               	UIPopupConfirm.singleton.Init(msg.text);
            
            // -- send answer depending on action and ready client
            if (msg.code == successCode && !msg.causesDisconnect)
            {
               	
               	
				switch (accountAction)
				{
					case NetworkActionLogin:
						if (RequestLogin(conn, userName, userPassword))
							ClientScene.Ready(conn);
						break;
					case NetworkActionRegister:
						RequestRegister(conn, userName, userPassword);
						break;
					case NetworkActionDelete:
						RequestSoftDelete(conn, userName, userPassword);
						break;
					case NetworkActionConfirm:
						RequestConfirm(conn, userName, userPassword);
						break;
					case NetworkActionSwitchServer:
						RequestSwitchServer(conn, userName);
						break;
					default:
						base.OnClientAuthenticated.Invoke(conn);
						break;
				}
			  	
			  	accountAction = 0;
               	
            }
            
            // -- disconnect and un-authenticate if anything went wrong
            if (msg.code == errorCode || msg.causesDisconnect)
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                manager.StopClient();
            }
            
        }
       
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================