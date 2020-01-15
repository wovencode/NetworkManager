// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;

namespace wovencode
{
	
	// ===================================================================================
	// BaseNetworkManager
	// ===================================================================================
	public abstract partial class BaseNetworkManager : Mirror.NetworkManager
	{
	
		[Header("Debug Helper")]
		public DebugHelper debug;
		public bool localHostAndPlay = true;
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================