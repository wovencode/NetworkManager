// =======================================================================================
// NetworkManager
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace wovencode
{

    // ===================================================================================
	// NetworkManager
	// ===================================================================================
	[RequireComponent(typeof(wovencode.NetworkAuthenticator))]
	[DisallowMultipleComponent]
	public partial class NetworkManager : Mirror.NetworkManager
	{
	
		public static Dictionary<string, GameObject> onlinePlayers = new Dictionary<string, GameObject>();
	
		[HideInInspector]public NetworkState state = NetworkState.Offline;
		
		[Header("Settings")]
		[Tooltip("Set a delay here to make players stay around for a little longer, even after they disconnect.")]
		public float disconnectDelay = 1;
		
		[Header("Events")]
		public UnityEvent onStartServer;
		public UnityEvent onStartClient;
		public UnityEvent onStopServer;
		
		[Header("Message Texts")]
		public string msgClientDisconnected 	= "Disconnected.";
		public string msgUserAlreadyOnline		= "User is already online!";
		
		// -------------------------------------------------------------------------------
		public override void Awake()
		{
			base.Awake();
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
		public override void OnStartClient()
		{
			onStartClient.Invoke();
			this.InvokeInstanceDevExtMethods("OnStartClient");
		}
		
		// -------------------------------------------------------------------------------
		public override void OnStartServer()
		{
			onStartServer.Invoke();
			this.InvokeInstanceDevExtMethods("OnStartServer");
		}
		
		// -------------------------------------------------------------------------------
		public override void OnStopServer()
		{
			onStopServer.Invoke();
			this.InvokeInstanceDevExtMethods("OnStopServer");
		}
		
		// -------------------------------------------------------------------------------
		public bool IsConnecting() => NetworkClient.active && !ClientScene.ready;
		
		// -------------------------------------------------------------------------------
		public override void OnClientConnect(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods("OnClientConnect");
		}
		
		// -------------------------------------------------------------------------------
		public override void OnServerConnect(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods("OnServerConnect");
		}
		
		// -------------------------------------------------------------------------------
		public override void OnClientSceneChanged(NetworkConnection conn) {
			this.InvokeInstanceDevExtMethods("OnClientSceneChanged");
		}

		// -------------------------------------------------------------------------------
		public void LoginPlayer(NetworkConnection conn, string _name)
		{
			if (!AccountLoggedIn(_name))
			{
				GameObject player = Database.singleton.LoadData(playerPrefab, _name);
				NetworkServer.AddPlayerForConnection(conn, player);
				state = NetworkState.Game;
			}
			else
				ServerSendError(conn, msgUserAlreadyOnline, true);
		}
		
		// -------------------------------------------------------------------------------
		public override void OnServerAddPlayer(NetworkConnection conn) {}
	
		
		// -------------------------------------------------------------------------------
		public override void OnServerDisconnect(NetworkConnection conn)
		{
			StartCoroutine(DoServerDisconnect(conn, disconnectDelay));
			this.InvokeInstanceDevExtMethods("OnServerDisconnect");
		}
		
		// -------------------------------------------------------------------------------
		IEnumerator<WaitForSeconds> DoServerDisconnect(NetworkConnection conn, float delay)
		{
			yield return new WaitForSeconds(delay);

			if (conn.identity != null)
			{
				Database.singleton.SaveData(conn.identity.gameObject, false);
				Debug.Log("[NetworkManager] Saved player: " + conn.identity.name);
			}

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
			UIPopupConfirm.singleton.Init(msgClientDisconnected);
			this.InvokeInstanceDevExtMethods("OnClientDisconnect");
		}
		
		// -------------------------------------------------------------------------------
		// Quit
		// universal quit function
		// -------------------------------------------------------------------------------
		public static void Quit()
		{
	#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
	#else
			Application.Quit();
	#endif
		}
	
		// ======================== PUBLIC EVENT METHODS =================================
		
		// -------------------------------------------------------------------------------
		// EventCreatePlayer
		// Invoked when the NetworkAuthenticator sucessfully registers a new player
		// -------------------------------------------------------------------------------
		public void EventCreatePlayer(string _name)
		{
			GameObject player = Instantiate(playerPrefab);
			player.name = _name;
			Database.singleton.CreateDefaultData(player);
			Database.singleton.SaveData(player, false);
			Destroy(player);
		}
		
		// -------------------------------------------------------------------------------
		// EventStartPlayer
		// Invoked when the clients player enters the scene
		// -------------------------------------------------------------------------------
		public void EventStartPlayer(GameObject player)
		{
			onlinePlayers[player.name] = player.gameObject;
			this.InvokeInstanceDevExtMethods("EventStartPlayer");
		}
	
		// -------------------------------------------------------------------------------
		// EventDestroyPlayer
		// Invoked when the clients player is destroyed / client disconnects
		// -------------------------------------------------------------------------------
		public void EventDestroyPlayer(GameObject player)
		{
			onlinePlayers.Remove(player.name);
			this.InvokeInstanceDevExtMethods("EventDestroyPlayer");
		}
	
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================