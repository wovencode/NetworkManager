// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace wovencode
{
	
	// -----------------------------------------------------------------------------------
	// NetworkManager_EventListeners
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class NetworkManager_EventListeners
	{
		
		
		public UnityEvent onStartServer;
        public UnityEvent onStopServer;
		public UnityEventGameObject onStartClient;
		public UnityEventConnection onLogoutClient;
		
		// ---- User
		public UnityEventString         onUserRegister;
		public UnityEventConnection     onUserLogin;
		public UnityEventString         onUserDelete;
		public UnityEventString         onUserConfirm;
		
		// ---- Player
		public UnityEventString         onPlayerRegister;
		public UnityEventGameObject     onPlayerLogin;
		public UnityEventString         onPlayerDelete;
		public UnityEventString         onPlayerSwitchServer;
		
	}
	
}

// =======================================================================================