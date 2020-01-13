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
    public partial class NetworkAuthenticator
    {
    	
    	// ======================= PUBLIC METHODS - USER =================================
    	
        // -------------------------------------------------------------------------------
        // RequestLoginUser
		// -------------------------------------------------------------------------------
		public override bool RequestLoginUser(NetworkConnection conn, string name, string password)
		{
			if (!base.RequestLoginUser(conn, name, password)) return false;
			
			UserLoginRequestMessage message = new UserLoginRequestMessage
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestRegisterUser
		// -------------------------------------------------------------------------------
		public override bool RequestRegisterUser(NetworkConnection conn, string name, string password)
		{
			if (!base.RequestRegisterUser(conn, name, password)) return false;
			
			UserRegisterRequestMessage message = new UserRegisterRequestMessage
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestSoftDeleteUser
		// -------------------------------------------------------------------------------
		public override bool RequestSoftDeleteUser(NetworkConnection conn, string name, string password, int _action=1)
		{
			if (!base.RequestSoftDeleteUser(conn, name, password)) return false;
			
			UserDeleteRequestMessage message = new UserDeleteRequestMessage
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestChangePasswordUser
		// -------------------------------------------------------------------------------
		public override bool RequestChangePasswordUser(NetworkConnection conn, string name, string oldpassword, string newpassword)
		{
			if (!base.RequestChangePasswordUser(conn, name, oldpassword, newpassword)) return false;
			
			UserChangePasswordRequestMessage message = new UserChangePasswordRequestMessage
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
        // RequestConfirmUser
		// -------------------------------------------------------------------------------
		public override bool RequestConfirmUser(NetworkConnection conn, string name, string password, int _action=1)
		{
			if (!base.RequestConfirmUser(conn, name, password)) return false;
			
			UserConfirmRequestMessage message = new UserConfirmRequestMessage
			{
				username = name,
				password = GenerateHash(name, password)
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// ======================= PUBLIC METHODS - PLAYER ================================
		
		
        // -------------------------------------------------------------------------------
        // RequestLoginPlayer
		// -------------------------------------------------------------------------------
		public override bool RequestLoginPlayer(NetworkConnection conn, string name)
		{
			if (!base.RequestLoginPlayer(conn, name)) return false;
			
			PlayerLoginRequestMessage message = new PlayerLoginRequestMessage
			{
				playername = name
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestRegisterPlayer
		// -------------------------------------------------------------------------------
		public override bool RequestRegisterPlayer(NetworkConnection conn, string name)
		{
			if (!base.RequestRegisterPlayer(conn, name)) return false;
			
			PlayerRegisterRequestMessage message = new PlayerRegisterRequestMessage
			{
				playername = name
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestSoftDeletePlayer
		// -------------------------------------------------------------------------------
		public override bool RequestSoftDeletePlayer(NetworkConnection conn, string name, int _action=1)
		{
			if (!base.RequestSoftDeletePlayer(conn, name)) return false;
			
			PlayerDeleteRequestMessage message = new PlayerDeleteRequestMessage
			{
				playername = name
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
        // RequestSwitchServerPlayer
		// -------------------------------------------------------------------------------
		public override bool RequestSwitchServerPlayer(NetworkConnection conn, string name, int _token=0)
		{
			
			_token = Tools.GenerateToken();
			
			if (!base.RequestSwitchServerPlayer(conn, name, _token)) return false;
			
			PlayerSwitchServerRequestMessage message = new PlayerSwitchServerRequestMessage
			{
				username = name,
				token = _token
			};
			
			conn.Send(message);
			
			return true;
			
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================