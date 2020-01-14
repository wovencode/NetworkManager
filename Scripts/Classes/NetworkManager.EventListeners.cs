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
	
		[Tooltip("DatabaseManager -> EventStartServer")]
		public UnityEvent onStartServer;
		[Tooltip("DatabaseManager -> EventStopServer")]
		public UnityEvent onStopServer;
		[Tooltip("DatabaseManager -> EventStartClient")]
		public UnityEventGameObject onStartClient;
		[Tooltip("DatabaseManager -> EventLogout")]
		public UnityEventConnection onLogoutClient;
		
	}
	
}

// =======================================================================================