// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace Wovencode.Network
{

    // ===================================================================================
	// NetworkAuthenticator
	// ===================================================================================
	[RequireComponent(typeof(Wovencode.Network.NetworkManager))]
    [DisallowMultipleComponent]
    public partial class NetworkAuthenticator : BaseNetworkAuthenticator
    {

    	[Header("Settings")]
		public bool checkApplicationVersion 				= true;
		
		[Header("System Texts")]
		public NetworkAuthenticator_Lang 					systemText;
		
		[Header("Event Listeners")]
		public NetworkAuthenticator_EventListeners 			eventListener;
		
		public static Wovencode.Network.NetworkAuthenticator singleton;
		
		// -------------------------------------------------------------------------------
		public void Awake()
		{
			singleton = this;
		}
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================