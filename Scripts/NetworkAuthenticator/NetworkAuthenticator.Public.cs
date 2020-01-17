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
	// NetworkAuthenticator
	// ===================================================================================
    public partial class NetworkAuthenticator
    {
/*
    	// ======================= PUBLIC METHODS - USER =================================
    	
        // -------------------------------------------------------------------------------
        // RequestUserLogin
        // @Client
		// -------------------------------------------------------------------------------
		public override bool RequestUserLogin(NetworkConnection conn, string name, string password)
		{
			if (!base.RequestUserLogin(conn, name, password)) return false;
			
			ClientMessageRequestUserLogin message = new ClientMessageRequestUserLogin
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestUserRegister
        // @Client
		// -------------------------------------------------------------------------------
		public override bool RequestUserRegister(NetworkConnection conn, string name, string password)
		{
			if (!base.RequestUserRegister(conn, name, password)) return false;
			
			ClientMessageRequestUserRegister message = new ClientMessageRequestUserRegister
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestUserDelete
        // @Client
		// -------------------------------------------------------------------------------
		public override bool RequestUserDelete(NetworkConnection conn, string name, string password, int action=1)
		{
			if (!base.RequestUserDelete(conn, name, password)) return false;
			
			ClientMessageRequestUserDelete message = new ClientMessageRequestUserDelete
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestUserChangePassword
        // @Client
		// -------------------------------------------------------------------------------
		public override bool RequestUserChangePassword(NetworkConnection conn, string name, string oldpassword, string newpassword)
		{
			if (!base.RequestUserChangePassword(conn, name, oldpassword, newpassword)) return false;
			
			ClientMessageRequestUserChangePassword message = new ClientMessageRequestUserChangePassword
			{
				username = name,
				oldPassword = GenerateHash(name, oldpassword),
				newPassword = GenerateHash(name, newpassword)
			};
			
			// reset player prefs on password change
			PlayerPrefs.SetString(Constants.PlayerPrefsPassword, "");
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestUserConfirm
        // @Client
		// -------------------------------------------------------------------------------
		public override bool RequestUserConfirm(NetworkConnection conn, string name, string password, int action=1)
		{
			if (!base.RequestUserConfirm(conn, name, password)) return false;
			
			ClientMessageRequestUserConfirm message = new ClientMessageRequestUserConfirm
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// ======================= PUBLIC METHODS - PLAYER ================================
		
        // -------------------------------------------------------------------------------
        // RequestPlayerLogin
        // @Client
		// -------------------------------------------------------------------------------
		public override bool RequestPlayerLogin(NetworkConnection conn, string name)
		{
			if (!base.RequestPlayerLogin(conn, name)) return false;
			
			ClientMessageRequestPlayerLogin message = new ClientMessageRequestPlayerLogin
			{
				playername = name
			};
			
			ClientScene.Ready(conn);
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestPlayerRegister
        // @Client
		// -------------------------------------------------------------------------------
		public override bool RequestPlayerRegister(NetworkConnection conn, string name)
		{
			if (!base.RequestPlayerRegister(conn, name)) return false;
			
			ClientMessageRequestPlayerRegister message = new ClientMessageRequestPlayerRegister
			{
				playername = name
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestPlayerDelete
        // @Client
		// -------------------------------------------------------------------------------
		public override bool RequestPlayerDelete(NetworkConnection conn, string name, int action=1)
		{
			if (!base.RequestPlayerDelete(conn, name)) return false;
			
			ClientMessageRequestPlayerDelete message = new ClientMessageRequestPlayerDelete
			{
				playername = name
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestPlayerSwitchServer
        // @Client
		// -------------------------------------------------------------------------------
		public override bool RequestPlayerSwitchServer(NetworkConnection conn, string name, int _token=0)
		{
			
			_token = Tools.GenerateToken();
			
			if (!base.RequestPlayerSwitchServer(conn, name, _token)) return false;
			
			ClientMessageRequestPlayerSwitchServer message = new ClientMessageRequestPlayerSwitchServer
			{
				username = name,
				token = _token
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
        // -------------------------------------------------------------------------------
 */              
    }

}

// =======================================================================================