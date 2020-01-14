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
		[Tooltip("NetworkManager -> EventRegisterUser")]
		public UnityEventString onUserRegister;
		[Tooltip("NetworkManager -> EventLoginUser / DatabaseManager -> EventLoginUser")]
		public UnityEventConnection onUserLogin;
		[Tooltip("NetworkManager -> EventLogoutUser / DatabaseManager -> EventLogoutUser")]
		public UnityEventString onUserDelete;
		[Tooltip("")]
		public UnityEventString onUserConfirm;
		
		// Player
		[Tooltip("")]
		public UnityEventString onPlayerRegister;
		[Tooltip("NetworkManager -> EventLoginPlayer")]
		public UnityEventGameObject onPlayerLogin;
		[Tooltip("")]
		public UnityEventString onPlayerDelete;
		[Tooltip("")]
		public UnityEventString onPlayerSwitchServer;
		
	}
	
}

// =======================================================================================