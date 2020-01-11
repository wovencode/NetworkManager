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
    
        // -------------------------------------------------------------------------------
        // OnAuthRequestMessage
        // @Client -> @Server
		// -------------------------------------------------------------------------------

		// -------------------------------------------------------------------------------
		public override bool RequestLogin(NetworkConnection _conn, string _name, string _password)
		{
			if (!base.RequestLogin(_conn, _name, _password)) return false;
			
			LoginRequestMessage message = new LoginRequestMessage
			{
				username = userName,
				password = GenerateHash()
			};
			
			_conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
		public override bool RequestRegister(NetworkConnection _conn, string _name, string _password)
		{
			if (!base.RequestRegister(_conn, _name, _password)) return false;
			
			RegisterRequestMessage message = new RegisterRequestMessage
			{
				username = userName,
				password = GenerateHash()
			};
			
			_conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
		public override bool RequestSoftDelete(NetworkConnection _conn, string _name, string _password, int _action=1)
		{
			if (!base.RequestSoftDelete(_conn, _name, _password)) return false;
			
			DeleteRequestMessage message = new DeleteRequestMessage
			{
				username = userName,
				password = GenerateHash()
			};
			
			_conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
		public override bool RequestConfirm(NetworkConnection _conn, string _name, string _password, int _action=1)
		{
			if (!base.RequestConfirm(_conn, _name, _password)) return false;
			
			ConfirmRequestMessage message = new ConfirmRequestMessage
			{
				username = userName,
				password = GenerateHash()
			};
			
			_conn.Send(message);
			
			return true;
			
		}
		
		// -------------------------------------------------------------------------------
		public override bool RequestSwitchServer(NetworkConnection _conn, string _name, int _token=0)
		{
			
			_token = Tools.GenerateToken();
			
			if (!base.RequestSwitchServer(_conn, _name, _token)) return false;
			
			SwitchServerRequestMessage message = new SwitchServerRequestMessage
			{
				username = userName,
				token = _token
			};
			
			_conn.Send(message);
			
			return true;
			
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================