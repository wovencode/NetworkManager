// =======================================================================================
// Wovencore
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
	
	// ================================= MESSAGES USER ===================================
	
	// -----------------------------------------------------------------------------------
	// UserLoginRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class UserLoginRequestMessage : MessageBase
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
	// UserRegisterRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class UserRegisterRequestMessage : MessageBase
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
	// UserDeleteRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class UserDeleteRequestMessage : MessageBase
	{
		public string username;
		public string password;
	}
	
	// -----------------------------------------------------------------------------------
	// UserChangePasswordRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class UserChangePasswordRequestMessage : MessageBase
	{
		public string username;
		public string oldPassword;
		public string newPassword;
	}
	
	// -----------------------------------------------------------------------------------
	// UserConfirmRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class UserConfirmRequestMessage : MessageBase
	{
		public string username;
		public string password;
	}
	
	// ================================ MESSAGES PLAYER ==================================
	
	// -----------------------------------------------------------------------------------
	// PlayerLoginRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class PlayerLoginRequestMessage : MessageBase
	{
		public string username;
		public string playername;
	}
	
	// -----------------------------------------------------------------------------------
	// PlayerRegisterRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class PlayerRegisterRequestMessage : MessageBase
	{
		public string username;
		public string playername;
	}
	
	// -----------------------------------------------------------------------------------
	// PlayerDeleteRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class PlayerDeleteRequestMessage : MessageBase
	{
		public string username;
		public string playername;
	}
	
	// -----------------------------------------------------------------------------------
	// PlayerSwitchServerRequestMessage
	// Requires Authorization
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public partial class PlayerSwitchServerRequestMessage : MessageBase
	{
		public string username;
		public string playername;
		public int zoneIndex;
		public int token;
	}
	
	// ================================ MESSAGES SERVER ==================================
	
	// -----------------------------------------------------------------------------------
	// ServerResponseMessage
	// A common response message sent from the server to the client
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerResponseMessage : MessageBase
	{
		public byte code;
		public string text;
		public bool causesDisconnect;
	}
	
	// -----------------------------------------------------------------------------------
	// ServerPlayerListMessage
	// 
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerPlayerListMessage : ServerResponseMessage
	{
	
		public PlayerInfo players;
		public int maxPlayers;
		
		public partial struct PlayerInfo
		{
			public string name;
		}
		
		
		
		
	}

	// -------------------------------------------------------------------------------
	
}