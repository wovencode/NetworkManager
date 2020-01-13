// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
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
		
		// User
		bool RequestLoginUser(NetworkConnection conn, string name, string password);
		bool RequestRegisterUser(NetworkConnection conn, string name, string password);
		bool RequestSoftDeleteUser(NetworkConnection conn, string name, string password, int action=1);
		bool RequestChangePasswordUser(NetworkConnection conn, string name, string oldpassword, string newpassword);
		bool RequestConfirmUser(NetworkConnection conn, string name, string password, int action=1);
		
		// Player
		bool RequestLoginPlayer(NetworkConnection conn, string name);
		bool RequestRegisterPlayer(NetworkConnection conn, string name);
		bool RequestSoftDeletePlayer(NetworkConnection conn, string name, int action=1);
		bool RequestSwitchServerPlayer(NetworkConnection conn, string name, int token=0);
		
	}
		
}

// =======================================================================================