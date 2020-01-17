// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using wovencode.Network;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace wovencode
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
    public partial class NetworkManager
    {
    
        // -------------------------------------------------------------------------------
        // OnStartClient
        // @Client
		// -------------------------------------------------------------------------------
        public override void OnStartClient()
        {
        	
            // ---- User
            NetworkClient.RegisterHandler<ServerMessageResponseUserLogin>(OnServerMessageResponseUserLogin);
            NetworkClient.RegisterHandler<ServerMessageResponseUserRegister>(OnServerMessageResponseUserRegister);
            NetworkClient.RegisterHandler<ServerMessageResponseUserDelete>(OnServerMessageResponseUserDelete);
            NetworkClient.RegisterHandler<ServerMessageResponseUserChangePassword>(OnServerMessageResponseUserChangePassword);
            NetworkClient.RegisterHandler<ServerMessageResponseUserConfirm>(OnServerMessageResponseUserConfirm);
            
            // ---- Player
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerLogin>(OnServerMessageResponsePlayerLogin);
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerRegister>(OnServerMessageResponsePlayerRegister);
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerDelete>(OnServerMessageResponsePlayerDelete);
            NetworkClient.RegisterHandler<ServerMessageResponsePlayerSwitchServer>(OnServerMessageResponsePlayerSwitchServer);
            
            this.InvokeInstanceDevExtMethods(nameof(OnStartClient));
            
        }
        
        // ===============================================================================
        // ============================= MESSAGE HANDLERS ================================
        // ===============================================================================
        
        // -------------------------------------------------------------------------------
		// OnServerMessageResponse
		// @Server -> @Client
		// -------------------------------------------------------------------------------
        void OnServerMessageResponse(NetworkConnection conn, ServerMessageResponse msg)
        {
    		
    		// -- show popup if error message is not empty
        	if (!String.IsNullOrWhiteSpace(msg.text))
               	UIPopupConfirm.singleton.Init(msg.text);
    		
        	// -- disconnect and un-authenticate if anything went wrong
            if (!msg.success || msg.causesDisconnect)
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                NetworkManager.singleton.StopClient();
            }
            
        }
        
        // ========================== MESSAGE HANDLERS - USER ============================
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponseUserLogin(NetworkConnection conn, ServerMessageResponseUserLogin msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponseUserRegister(NetworkConnection conn, ServerMessageResponseUserRegister msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponseUserDelete(NetworkConnection conn, ServerMessageResponseUserDelete msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponseUserChangePassword(NetworkConnection conn, ServerMessageResponseUserChangePassword msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponseUserConfirm(NetworkConnection conn, ServerMessageResponseUserConfirm msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }
        
        // ======================== MESSAGE HANDLERS - PLAYER ============================
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponsePlayerLogin(NetworkConnection conn, ServerMessageResponsePlayerLogin msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponsePlayerRegister(NetworkConnection conn, ServerMessageResponsePlayerRegister msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponsePlayerDelete(NetworkConnection conn, ServerMessageResponsePlayerDelete msg)
        {
        	
        	OnServerMessageResponse(conn, msg);
        }
        
        // -------------------------------------------------------------------------------
        void OnServerMessageResponsePlayerSwitchServer(NetworkConnection conn, ServerMessageResponsePlayerSwitchServer msg)
        {
        	int token = 0;
        	//RequestPlayerSwitchServer(conn, userName, token);
        	OnServerMessageResponse(conn, msg);
        }      
        
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================