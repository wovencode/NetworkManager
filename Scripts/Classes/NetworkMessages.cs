// =======================================================================================
// NetworkMessages
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using Mirror;

namespace wovencode
{

	// -----------------------------------------------------------------------------------
	// ErrorMsg
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ErrorMsg : MessageBase
	{
   		public string text;
   		public bool causesDisconnect;
	}
	
	// -----------------------------------------------------------------------------------
	// LoginRequestMessage
	// Unauthorized -> results in Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class AuthRequestMessage : MessageBase
	{
		public string clientVersion;
	}
	
	// -----------------------------------------------------------------------------------
	// LoginRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class LoginRequestMessage : MessageBase
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
	// RegisterRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class RegisterRequestMessage : MessageBase
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
	// DeleteRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class DeleteRequestMessage : MessageBase
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
	// ConfirmRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class ConfirmRequestMessage : MessageBase
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
	// SwitchServerRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class SwitchServerRequestMessage : MessageBase
	{
		public string username;
		public int token;
	}
	
	// -----------------------------------------------------------------------------------
	// ServerResponseMessage
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerResponseMessage : MessageBase
	{
		public byte code;
		public string text;
		public bool causesDisconnect;
	}

	
	
	// -------------------------------------------------------------------------------
	
}