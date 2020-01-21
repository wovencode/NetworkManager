// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;

namespace Wovencode.Network
{
	
	// ===================================================================================
	// BaseNetworkAuthenticator
	// ===================================================================================
	public abstract partial class BaseNetworkAuthenticator : Mirror.NetworkAuthenticator
	{
		
		[Header("Debug Helper")]
		public DebugHelper debug;
		
	}

}

// =======================================================================================