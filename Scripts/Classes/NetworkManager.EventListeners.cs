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
	}
	
}

// =======================================================================================