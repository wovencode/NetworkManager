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
		
    	// =========================== PUBLIC METHODS ====================================
    	
		// -------------------------------------------------------------------------------
		public virtual bool RequestLogin(NetworkConnection _conn, string _name, string _password)
		{
			return (Tools.IsAllowedName(_name) && Tools.IsAllowedPassword(_password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestRegister(NetworkConnection _conn, string _name, string _password)
		{
			return (Tools.IsAllowedName(_name) && Tools.IsAllowedPassword(_password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestSoftDelete(NetworkConnection _conn, string _name, string _password, int _action=1)
		{
			return (Tools.IsAllowedName(_name) && Tools.IsAllowedPassword(_password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestConfirm(NetworkConnection _conn, string _name, string _password, int _action=1)
		{
			return (Tools.IsAllowedName(_name) && Tools.IsAllowedPassword(_password));
		}
		
		// -------------------------------------------------------------------------------
		public virtual bool RequestSwitchServer(NetworkConnection _conn, string _name, int _token)
		{
			return (Tools.IsAllowedName(_name) && Tools.IsAllowedToken(_token));
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================