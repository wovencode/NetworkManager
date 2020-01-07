// =======================================================================================
// NetworkMessages
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using Mirror;

namespace wovencode
{

	// -------------------------------------------------------------------------------
	// ErrorMsg
	// @Server -> @Client
	// -------------------------------------------------------------------------------
	public partial class ErrorMsg : MessageBase
	{
   		public string text;
   		public bool causesDisconnect;
	}
	
	// -------------------------------------------------------------------------------
	// AuthRequestMessage
	// @Client -> @Server
	// -------------------------------------------------------------------------------
	public partial class AuthRequestMessage : MessageBase
	{
		public string authUsername;
		public string authPassword;
		public byte authAction;
		public string authVersion;
	}

	// -------------------------------------------------------------------------------
	// AuthResponseMessage
	// @Server -> @Client
	// -------------------------------------------------------------------------------
	public partial class AuthResponseMessage : MessageBase
	{
		public byte code;
		public string text;
		public bool causesDisconnect;
	}

	// -------------------------------------------------------------------------------
	// LoginMessage
	// @Client -> @Server
	// -------------------------------------------------------------------------------
	public partial class LoginMessage : MessageBase
	{
		public string authUsername;
		public string authPassword;
		//public int factionId;
	}
	
	// -------------------------------------------------------------------------------
	
}