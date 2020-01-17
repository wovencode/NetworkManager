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
	// ErrorMsg
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public partial class ErrorMsg : MessageBase
	{
   		public string text;
   		public bool causesDisconnect;
	}
	
	// -------------------------------------------------------------------------------
	
}