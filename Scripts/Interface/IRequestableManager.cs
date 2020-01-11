// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
//
//
// =======================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using wovencode;

namespace wovencode
{

	// ===================================================================================
	// IRequestableManager
	// ===================================================================================
	public interface IRequestableManager
	{
		
		bool RequestLogin(NetworkConnection _conn, string _name, string _password);
		bool RequestRegister(NetworkConnection _conn, string _name, string _password);
		bool RequestSoftDelete(NetworkConnection _conn, string _name, string _password, int _action=1);
		bool RequestConfirm(NetworkConnection _conn, string _name, string _password, int _action=1);
		bool RequestSwitchServer(NetworkConnection _conn, string _name, int _token=0);
		
	}
		
}

// =======================================================================================