// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Database;
using Wovencode.Network;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Wovencode.Network
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
	public partial class NetworkManager
	{
	
		// =================== PUBLIC METHODS FOR AUTHENTICATOR ==========================
		
		// -------------------------------------------------------------------------------
		// LoginUser
		// @NetworkAuthenticator
		// -------------------------------------------------------------------------------
		public void LoginUser(NetworkConnection conn, string name)
		{
			onlineUsers[conn] = name;
			state = NetworkState.Lobby;
			this.InvokeInstanceDevExtMethods(nameof(LoginUser));
		}
		
		// -------------------------------------------------------------------------------
		// LoginPlayer
		// @NetworkAuthenticator
		// -------------------------------------------------------------------------------
		public void LoginPlayer(NetworkConnection conn, string _name)
		{
			if (!AccountLoggedIn(_name))
			{
				GameObject player = DatabaseManager.singleton.LoadData(playerPrefab, _name);
				NetworkServer.AddPlayerForConnection(conn, player);
				onlinePlayers[player.name] = player;
				state = NetworkState.Game;
				
				this.InvokeInstanceDevExtMethods(nameof(LoginPlayer));
			}
			else
				ServerSendError(conn, systemText.userAlreadyOnline, true);
		}
		
		// -------------------------------------------------------------------------------
		// RegisterPlayer
		// 
		// @NetworkAuthenticator
		// -------------------------------------------------------------------------------
		public void RegisterPlayer(string playername)
		{
			GameObject player = Instantiate(playerPrefab);
			player.name = playername;
			DatabaseManager.singleton.CreateDefaultData(player);
			DatabaseManager.singleton.SaveDataPlayer(player, false);
			Destroy(player);
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================