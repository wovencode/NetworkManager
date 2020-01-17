// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;

namespace wovencode
{
	
	// ===================================================================================
	// BaseNetworkManager
	// ===================================================================================
	public abstract partial class BaseNetworkManager
	{
		
		
    	// ======================= PUBLIC METHODS - USER =================================
    	
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserLogin(NetworkConnection conn, string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserRegister(NetworkConnection conn, string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserDelete(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserChangePassword(NetworkConnection conn, string name, string oldpassword, string newpassword)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(oldpassword) && Tools.IsAllowedPassword(newpassword) && oldpassword != newpassword);
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestUserConfirm(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// ======================= PUBLIC METHODS - PLAYER =================================
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerLogin(NetworkConnection conn, string name)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerRegister(NetworkConnection conn, string name)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerDelete(NetworkConnection conn, string name, int _action=1)
		{
			return (Tools.IsAllowedName(name));
		}
				
		// -------------------------------------------------------------------------------
		protected virtual bool RequestPlayerSwitchServer(NetworkConnection conn, string name, int _token)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================