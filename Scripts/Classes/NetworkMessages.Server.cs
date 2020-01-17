// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode.Network;
using wovencode;
using Mirror;

namespace wovencode.Network
{

	// -----------------------------------------------------------------------------------
	// ServerMessageResponse
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponse : MessageBase
	{
		public bool success;
		public string text;
		public bool causesDisconnect;
	}

	// ================================= MESSAGES AUTH ===================================
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseAuth
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseAuth : ServerMessageResponse
	{
		
	}

	// ================================== MESSAGES USER ==================================
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserLogin
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserLogin : ServerMessageResponse
	{
	
		public PlayerInfo players;
		public int maxPlayers;
		
		public partial struct PlayerInfo
		{
			public string name;
		}
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserRegister
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserRegister : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserDelete
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserDelete : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserChangePassword
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserChangePassword : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponseUserConfirm
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponseUserConfirm : ServerMessageResponse
	{
		
	}
	
	// ================================ MESSAGES PLAYER ==================================
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponsePlayerLogin
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponsePlayerLogin : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponsePlayerRegister
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponsePlayerRegister : ServerMessageResponse
	{
		
	}	
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponsePlayerDelete
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponsePlayerDelete : ServerMessageResponse
	{
		
	}
	
	// -----------------------------------------------------------------------------------
	// ServerMessageResponsePlayerSwitchServer
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ServerMessageResponsePlayerSwitchServer : ServerMessageResponse
	{
		
	}
	
	// -------------------------------------------------------------------------------
	
}