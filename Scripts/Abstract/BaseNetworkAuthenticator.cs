﻿// =======================================================================================
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
	// BaseNetworkAuthenticator
	// ===================================================================================
	public abstract partial class BaseNetworkAuthenticator : Mirror.NetworkAuthenticator
	{
		
		[Header("Debug Helper")]
		public DebugHelper debug;
		
/*
    	// ======================= PUBLIC METHODS - USER =================================
    	
		// -------------------------------------------------------------------------------
		public virtual bool RequestUserLogin(NetworkConnection conn, string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestUserRegister(NetworkConnection conn, string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestUserDelete(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestUserChangePassword(NetworkConnection conn, string name, string oldpassword, string newpassword)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(oldpassword) && Tools.IsAllowedPassword(newpassword) && oldpassword != newpassword);
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestUserConfirm(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// ======================= PUBLIC METHODS - PLAYER =================================
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestPlayerLogin(NetworkConnection conn, string name)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestPlayerRegister(NetworkConnection conn, string name)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestPlayerDelete(NetworkConnection conn, string name, int _action=1)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestPlayerConfirm(NetworkConnection conn, string name, int _action=1)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestPlayerSwitchServer(NetworkConnection conn, string name, int _token)
		{
			return (Tools.IsAllowedName(name));
		}
*/
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================