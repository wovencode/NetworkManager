// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using wovencode.Network;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Mirror;

namespace wovencode
{

    // ===================================================================================
	// NetworkAuthenticator
	// ===================================================================================
	[RequireComponent(typeof(wovencode.NetworkManager))]
    [DisallowMultipleComponent]
    public partial class NetworkAuthenticator : BaseNetworkAuthenticator
    {

    	[Header("Settings")]
		public bool checkApplicationVersion 				= true;
		
		[Header("System Texts")]
		public NetworkAuthenticator_Lang 					systemText;
		
		[Header("Event Listeners")]
		public NetworkAuthenticator_EventListeners 			eventListener;
		
		
		public static wovencode.NetworkAuthenticator singleton;
		
		
		
		// -------------------------------------------------------------------------------
		public void Awake()
		{
			singleton = this;
		}
		
		// -------------------------------------------------------------------------------
		// GenerateHash
		// Helper function to generate a hash from the current userName, salt & account name
		// -------------------------------------------------------------------------------
		/*protected string GenerateHash(string encryptText, string saltText)
		{
			return Tools.PBKDF2Hash(encryptText, userNameSalt + saltText);
		}*/
		
        // -------------------------------------------------------------------------------
               
    }
}

// =======================================================================================