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
	// BaseNetworkAuthenticator
	// ===================================================================================
	public abstract partial class BaseNetworkAuthenticator : Mirror.NetworkAuthenticator, IRequestableManager
	{
		
		[Header("Debug Mode")]
		public bool debugMode;
		
    	// ======================= PUBLIC METHODS - USER =================================
    	
		// -------------------------------------------------------------------------------
		public virtual bool RequestLoginUser(NetworkConnection conn, string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestRegisterUser(NetworkConnection conn, string name, string password)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestSoftDeleteUser(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestChangePasswordUser(NetworkConnection conn, string name, string oldpassword, string newpassword)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(oldpassword) && Tools.IsAllowedPassword(newpassword) && oldpassword != newpassword);
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestConfirmUser(NetworkConnection conn, string name, string password, int _action=1)
		{
			return (Tools.IsAllowedName(name) && Tools.IsAllowedPassword(password));
		}
		
		// ======================= PUBLIC METHODS - PLAYER =================================
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestLoginPlayer(NetworkConnection conn, string name)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestRegisterPlayer(NetworkConnection conn, string name)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestSoftDeletePlayer(NetworkConnection conn, string name, int _action=1)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestConfirmPlayer(NetworkConnection conn, string name, int _action=1)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestSwitchServerPlayer(NetworkConnection conn, string name, int _token)
		{
			return (Tools.IsAllowedName(name));
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================