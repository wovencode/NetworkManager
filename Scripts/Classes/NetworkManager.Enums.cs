// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode.Network;
using Wovencode;
using System;

namespace Wovencode.Network
{

	public enum NetworkState 	{
		Offline,
		Lobby,
		Game
	}
	
	public enum NetworkType
	{
		Client,
		Server,
		HostAndPlay,
	}
	
}