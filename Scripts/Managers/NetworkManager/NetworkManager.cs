// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using Wovencode.Database;
using Wovencode.UI;
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
	[RequireComponent(typeof(ConfigurationManager))]
	[RequireComponent(typeof(Wovencode.Network.NetworkAuthenticator))]
	[DisallowMultipleComponent]
	public partial class NetworkManager : BaseNetworkManager
	{
		
		[Header("Security")]
    	public string userNameSalt = "at_least_16_byte";
		
		[Header("Servers")]
		public List<ServerInfo> serverList = new List<ServerInfo>()
		{
        	new ServerInfo{name="Local", ip="localhost"}
    	};
		
		[Header("System Texts")]
		public NetworkManager_Lang systemText;
		
		[Header("Event Listeners")]
		public NetworkManager_EventListeners eventListener;
		
		public static new Wovencode.Network.NetworkManager singleton;
		
#if wPLAYER
		public List<GameObject> playerPrefabs;
#endif

		public static Dictionary<string, GameObject> onlinePlayers = new Dictionary<string, GameObject>();
		protected Dictionary<NetworkConnection, string> onlineUsers = new Dictionary<NetworkConnection, string>();
		
		[HideInInspector]public string userName 			= "";
        [HideInInspector]public string userPassword			= "";
        [HideInInspector]public string newPassword			= "";
        
        [HideInInspector]public List<PlayerPreview> playerPreviews = new List<PlayerPreview>();
		[HideInInspector]public int maxPlayers				= 0;
		
		[HideInInspector]public NetworkState state = NetworkState.Offline;
		
		// -------------------------------------------------------------------------------
		public override void Awake()
		{
			singleton = this;
			base.Awake();
			
			switch (networkType)
			{
				case NetworkType.Server:
					StartServer();
					break;
				case NetworkType.HostAndPlay:
					StartHost();
					break;
				default:
					StartClient();
					break;
			}

#if wPLAYER
			FilterPlayerPrefabs();
#else
			debug.LogWarning("[NetworkManager] No players added to playerPrefabs list (Define #wPLAYER missing)");
#endif

		}

		// -------------------------------------------------------------------------------
		public override void Start()
		{
			base.Start();
			this.InvokeInstanceDevExtMethods("Start");
		}
		
		// -------------------------------------------------------------------------------
		void Update()
		{
			if (ClientScene.localPlayer != null)
				state = NetworkState.Game;
		}
		
		// -------------------------------------------------------------------------------
		public bool AccountLoggedIn(string _name)
		{
			foreach (KeyValuePair<string, GameObject> player in onlinePlayers)
				if (player.Value.name == _name) return true;
			
			return false;
		}
		
		// -------------------------------------------------------------------------------
		public string UserName(NetworkConnection conn)
		{
			foreach (KeyValuePair<NetworkConnection, string> user in onlineUsers)
				if (user.Key == conn) return user.Value;
			
			return "";

		}
		
		// -------------------------------------------------------------------------------
		public void ServerSendError(NetworkConnection conn, string error, bool disconnect)
		{
			conn.Send(new ErrorMsg{text=error, causesDisconnect=disconnect});
		}
		
		// -------------------------------------------------------------------------------
		void OnClientError(NetworkConnection conn, ErrorMsg message)
		{
			
			if (!String.IsNullOrWhiteSpace(message.text))
				UIPopupConfirm.singleton.Init(message.text);
			
			if (message.causesDisconnect)
			{
				conn.Disconnect();
				if (NetworkServer.active) StopHost();
			}
		}
		
		// -------------------------------------------------------------------------------
		public override void OnStopServer()
		{
#if wDB
			DatabaseManager.singleton.Destruct();
#endif
			eventListener.onStopServer.Invoke();
			this.InvokeInstanceDevExtMethods(nameof(OnStopServer));
		}
		
		// -------------------------------------------------------------------------------
		// GenerateHash
		// Helper function to generate a hash from the current userName, salt & account name
		// -------------------------------------------------------------------------------
		protected string GenerateHash(string encryptText, string saltText)
		{
			return Tools.PBKDF2Hash(encryptText, userNameSalt + saltText);
		}
		
		// -------------------------------------------------------------------------------
		public bool IsConnecting() => NetworkClient.active && !ClientScene.ready;
		
		// -------------------------------------------------------------------------------
		public override void OnClientConnect(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnClientConnect));
		}
		
		// -------------------------------------------------------------------------------
		public override void OnServerConnect(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnServerConnect));
		}
		
		// -------------------------------------------------------------------------------
		public override void OnClientSceneChanged(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods(nameof(OnClientSceneChanged));
		}

		// -------------------------------------------------------------------------------
		public override void OnServerAddPlayer(NetworkConnection conn) {}
	
		// -------------------------------------------------------------------------------
		public override void OnServerDisconnect(NetworkConnection conn)
		{

#if wDB
			DatabaseManager.singleton.LogoutUser(UserName(conn));
#endif
			if (conn.identity != null)
			{
			
				eventListener.onLogoutClient.Invoke(conn);
				
				debug.Log("[NetworkManager] Logged out player: " + conn.identity.name);
				
				if (conn.identity.gameObject != null)
				{
					string name = conn.identity.gameObject.name;
					onlinePlayers.Remove(name);
				}
					
			}
			
			onlineUsers.Remove(conn);
			base.OnServerDisconnect(conn);
			
		}
		
		// -------------------------------------------------------------------------------
		// OnClientDisconnect
		// @Client
		// -------------------------------------------------------------------------------
		public override void OnClientDisconnect(NetworkConnection conn)
		{
			base.OnClientDisconnect(conn);
			state = NetworkState.Offline;
			UIPopupConfirm.singleton.Init(systemText.clientDisconnected);
			this.InvokeInstanceDevExtMethods(nameof(OnClientDisconnect));
		}
		
		// -------------------------------------------------------------------------------
		// Quit
		// universal quit function
		// -------------------------------------------------------------------------------
		public void Quit()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		// -------------------------------------------------------------------------------
		// FilterPlayerPrefabs
		// -------------------------------------------------------------------------------
#if wPLAYER
		protected void FilterPlayerPrefabs()
    	{
       		
       		playerPrefabs = new List<GameObject>();
        	
        	foreach (GameObject prefab in spawnPrefabs)
        	{
            	PlayerComponent player = prefab.GetComponent<PlayerComponent>();
            	if (player != null)
               		playerPrefabs.Add(prefab);
        	}
        	
    	}
#endif

		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================