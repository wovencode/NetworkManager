// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using UnityEngine;
using wovencode;
using System;

namespace wovencode
{
	
	// -----------------------------------------------------------------------------------
	// NetworkAuthenticator_EventListeners
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class NetworkAuthenticator_EventListeners
	{
		
		// User
		public UnityEventString         onUserRegister;
		public UnityEventConnection     onUserLogin;
		public UnityEventString         onUserDelete;
		public UnityEventString         onUserConfirm;
		
		// Player
		public UnityEventString         onPlayerRegister;
		public UnityEventGameObject     onPlayerLogin;
		public UnityEventString         onPlayerDelete;
		public UnityEventString         onPlayerSwitchServer;
		
	}
	
}

// =======================================================================================