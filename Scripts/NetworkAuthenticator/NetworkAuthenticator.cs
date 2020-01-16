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
    
		[Header("Security")]
    	public string userNameSalt 							= "at_least_16_byte";
    	
    	[Header("Settings")]
		public bool checkApplicationVersion 				= true;
		
		[Header("System Texts")]
		public NetworkAuthenticator_Lang 					systemText;
		
		[Header("Event Listeners")]
		public NetworkAuthenticator_EventListeners 			eventListener;
		
		[HideInInspector]public string userName 			= "";
        [HideInInspector]public string userPassword			= "";
        [HideInInspector]public string newPassword			= "";
		[HideInInspector]public NetworkAction userAction 	= NetworkAction.None;
		
		protected const byte successCode 	= 100;
		protected const byte errorCode 		= 200;
		
		// -------------------------------------------------------------------------------
		// GenerateHash
		// Helper function to generate a hash from the current userName, salt & account name
		// -------------------------------------------------------------------------------
		protected string GenerateHash(string encryptText, string saltText)
		{
			return Tools.PBKDF2Hash(encryptText, userNameSalt + saltText);
		}
		
		// -------------------------------------------------------------------------------
		// OnStartServer
		// @Server
		// -------------------------------------------------------------------------------
        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
            
            // User
            NetworkServer.RegisterHandler<UserLoginRequestMessage>(OnUserLoginRequestMessage, true);
            NetworkServer.RegisterHandler<UserRegisterRequestMessage>(OnUserRegisterRequestMessage, true);
            NetworkServer.RegisterHandler<UserDeleteRequestMessage>(OnUserDeleteRequestMessage, true);
            NetworkServer.RegisterHandler<UserChangePasswordRequestMessage>(OnUserChangePasswordRequestMessage, true);
            NetworkServer.RegisterHandler<UserConfirmRequestMessage>(OnUserConfirmRequestMessage, true);
            
            // Player
            NetworkServer.RegisterHandler<PlayerLoginRequestMessage>(OnPlayerLoginRequestMessage, true);
            NetworkServer.RegisterHandler<PlayerRegisterRequestMessage>(OnPlayerRegisterRequestMessage, true);
            NetworkServer.RegisterHandler<PlayerDeleteRequestMessage>(OnPlayerDeleteRequestMessage, true);
            NetworkServer.RegisterHandler<PlayerSwitchServerRequestMessage>(OnPlayerSwitchServerRequestMessage, true);
        }
        
        // -------------------------------------------------------------------------------
        // OnStartClient
        // @Client
		// -------------------------------------------------------------------------------
        public override void OnStartClient()
        {
            NetworkClient.RegisterHandler<ServerResponseMessage>(OnServerResponseMessage, false);
            NetworkClient.RegisterHandler<ServerPlayerListMessage>(OnServerPlayerListMessage, true);
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
				message.text = systemText.versionMismatch;
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
        
        // ========================== MESSAGE HANDLERS - USER ============================
        
        // -------------------------------------------------------------------------------
        // OnUserLoginRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------      
        void OnUserLoginRequestMessage(NetworkConnection conn, UserLoginRequestMessage msg)
		{
			
			/*
			TODO: 
			
			ServerPlayerListMessage
			*/
			
			ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryLoginUser(msg.username, msg.password))
			{
				NetworkManager.singleton.LoginUser(conn, msg.username);
				eventListener.onUserLogin.Invoke(conn);
				message.text = systemText.userLoginSuccess;
			}
			else
			{
				message.text = systemText.userLoginFailure;
				message.code = errorCode;
			}
					
			conn.Send(message);
			
		}
		
       	// -------------------------------------------------------------------------------
        // OnUserRegisterRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnUserRegisterRequestMessage(NetworkConnection conn, UserRegisterRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= true
			};
        	
        	if (DatabaseManager.singleton.TryRegisterUser(msg.username, msg.password))
			{
				DatabaseManager.singleton.SaveDataUser(msg.username, false);
				eventListener.onUserRegister.Invoke(msg.username);
				message.text = systemText.userRegisterSuccess;
			}
			else
			{
				message.text = systemText.userRegisterFailure;
				message.code = errorCode;
			}

        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnUserDeleteRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnUserDeleteRequestMessage(NetworkConnection conn, UserDeleteRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= true
			};
        	
        	if (DatabaseManager.singleton.TrySoftDeleteUser(msg.username, msg.password))
			{
				message.text = systemText.userDeleteSuccess;
				eventListener.onUserDelete.Invoke(msg.username);
			}
			else
			{
				message.text = systemText.userDeleteFailure;
				message.code = errorCode;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnUserChangePasswordRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnUserChangePasswordRequestMessage(NetworkConnection conn, UserChangePasswordRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= true
			};
        	
        	if (DatabaseManager.singleton.TryChangePasswordUser(msg.username, msg.oldPassword, msg.newPassword))
			{
				message.text = systemText.userChangePasswordSuccess;
			}
			else
			{
				message.text = systemText.userChangePasswordFailure;
				message.code = errorCode;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnUserConfirmRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnUserConfirmRequestMessage(NetworkConnection conn, UserConfirmRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryConfirmUser(msg.username, msg.password))
			{
				message.text = systemText.userConfirmSuccess;
				eventListener.onUserConfirm.Invoke(msg.username);
			}
			else
			{
				message.text = systemText.userConfirmFailure;
				message.code = errorCode;
			}
					
        	conn.Send(message);
        	
        }
        
        // ========================= MESSAGE HANDLERS - PLAYER ============================
        
        // -------------------------------------------------------------------------------
        // OnPlayerLoginRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------      
        void OnPlayerLoginRequestMessage(NetworkConnection conn, PlayerLoginRequestMessage msg)
		{
			
			ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryLoginPlayer(msg.playername, msg.username))
			{
				NetworkManager.singleton.LoginPlayer(conn, msg.playername);
				
				message.text = systemText.playerLoginSuccess;
				eventListener.onPlayerLogin.Invoke(conn.identity.gameObject);
			}
			else
			{
				message.text = systemText.playerLoginFailure;
				message.code = errorCode;
			}
					
			conn.Send(message);
			
		}
		
       	// -------------------------------------------------------------------------------
        // OnPlayerRegisterRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnPlayerRegisterRequestMessage(NetworkConnection conn, PlayerRegisterRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= true
			};
        	
        	if (DatabaseManager.singleton.TryRegisterPlayer(msg.playername, msg.username))
			{
				NetworkManager.singleton.RegisterPlayer(msg.playername);
				message.text = systemText.playerRegisterSuccess;
				eventListener.onPlayerRegister.Invoke(msg.playername);
			}
			else
			{
				message.text = systemText.playerRegisterFailure;
				message.code = errorCode;
			}
					
        	conn.Send(message);
        	
        }
        
        // -------------------------------------------------------------------------------
        // OnPlayerDeleteRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
        void OnPlayerDeleteRequestMessage(NetworkConnection conn, PlayerDeleteRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= true
			};
        	
        	if (DatabaseManager.singleton.TrySoftDeletePlayer(msg.playername, msg.username))
			{
				message.text = systemText.playerDeleteSuccess;
				eventListener.onPlayerDelete.Invoke(msg.playername);
			}
			else
			{
				message.text = systemText.playerDeleteFailure;
				message.code = errorCode;
			}
					
        	conn.Send(message);
        	
        }
        
        
        // -------------------------------------------------------------------------------
        // OnPlayerSwitchServerRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------    
       	void OnPlayerSwitchServerRequestMessage(NetworkConnection conn, PlayerSwitchServerRequestMessage msg)
        {
        	
        	ServerResponseMessage message = new ServerResponseMessage
			{
				code 				= successCode,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TrySwitchServerPlayer(msg.username, msg.token))
			{
				message.text = systemText.playerSwitchServerSuccess;
				eventListener.onPlayerSwitchServer.Invoke(msg.username);
			}
			else
			{
				message.text = systemText.playerSwitchServerFailure;
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
               	
               	base.OnClientAuthenticated.Invoke(conn);
               	
				switch (userAction)
				{
					
					// -- User
					case NetworkAction.LoginUser:
						RequestLoginUser(conn, userName, userPassword);
						break;
					case NetworkAction.RegisterUser:
						RequestRegisterUser(conn, userName, userPassword);
						break;
					case NetworkAction.DeleteUser:
						RequestSoftDeleteUser(conn, userName, userPassword);
						break;
					case NetworkAction.ChangePasswordUser:
						RequestChangePasswordUser(conn, userName, userPassword, newPassword);
						break;
					case NetworkAction.ConfirmUser:
						RequestConfirmUser(conn, userName, userPassword);
						break;
					
					// -- Player
					case NetworkAction.LoginPlayer:
						if (RequestLoginPlayer(conn, userName))
							ClientScene.Ready(conn);
						break;
					case NetworkAction.RegisterPlayer:
						RequestRegisterPlayer(conn, userName);
						break;
					case NetworkAction.DeletePlayer:
						RequestSoftDeletePlayer(conn, userName);
						break;
					case NetworkAction.SwitchServerPlayer:
						RequestSwitchServerPlayer(conn, userName);
						break;
				}
			  	
			  	userAction = 0;
               	
            }
            
            // -- disconnect and un-authenticate if anything went wrong
            if (msg.code == errorCode || msg.causesDisconnect)
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                NetworkManager.singleton.StopClient();
            }
            
        }
        
        // -------------------------------------------------------------------------------
		// OnServerPlayerListMessage
		// @Server -> @Client
		// -------------------------------------------------------------------------------
        void OnServerPlayerListMessage(NetworkConnection conn, ServerPlayerListMessage msg)
        {
        
        }
        
       
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================