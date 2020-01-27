// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using System;
using UnityEngine;
using Wovencode;
using Wovencode.Network;
using MIrror;

namespace Wovencode.Network
{
	
	// ===================================================================================
	// NetworkStartPosition
	// ===================================================================================
	[DisallowMultipleComponents]
	public partial class NetworkStartPosition : Mirror.NetworkStartPosition
	{
		
		public GameObject[] playerPrefab;
		public ArchetypeTemplate[] archeType;
		

	}
		
}