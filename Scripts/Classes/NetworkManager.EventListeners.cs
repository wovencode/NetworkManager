// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode.Network;
using Wovencode;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Wovencode.Network
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