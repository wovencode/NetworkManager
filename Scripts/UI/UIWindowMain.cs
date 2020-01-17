// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace wovencode
{

	// ===================================================================================
	// UIWindowMain
	// ===================================================================================
	public partial class UIWindowMain : UIRoot
	{
			
		[Header("Windows")]
		public UIWindowLoginUser 			loginWindow;
		public UIWindowRegisterUser 		registerWindow;
		public UIWindowChangePasswordUser 	changePasswordWindow;
		public UIWindowDeleteUser 			deleteWindow;
		
		[Header("Dropdown")]
		public Dropdown serverDropdown;
		
		[Header("Buttons")]
		public Button loginButton;
		public Button registerButton;
		public Button changePasswordButton;
		public Button deleteButton;
		public Button serverButton;
		public Button quitButton;
		
		[Header("Settings")]
		public bool rememberServer;
		
		public static UIWindowMain singleton;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		void Start()
		{
			if (!rememberServer) return;
			
			if (PlayerPrefs.HasKey(Constants.PlayerPrefsLastServer))
			{
				string lastServer = PlayerPrefs.GetString(Constants.PlayerPrefsLastServer, "");
				serverDropdown.value = NetworkManager.singleton.serverList.FindIndex(s => s.name == lastServer);
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
		void OnDestroy()
		{
			if (!rememberServer) return;
			PlayerPrefs.SetString(Constants.PlayerPrefsLastServer, serverDropdown.captionText.text);
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (NetworkManager.singleton.state == NetworkState.Offline)
			{
				
				loginButton.interactable = NetworkManager.singleton.CanClick();
				loginButton.onClick.SetListener(() => { loginWindow.Show(); });
				
				registerButton.interactable = NetworkManager.singleton.CanClick();
				registerButton.onClick.SetListener(() => { registerWindow.Show(); });
				
				changePasswordButton.interactable = NetworkManager.singleton.CanClick();
				changePasswordButton.onClick.SetListener(() => { changePasswordWindow.Show(); });
				
				deleteButton.interactable = NetworkManager.singleton.CanClick();
				deleteButton.onClick.SetListener(() => { deleteWindow.Show(); });
			
				serverButton.interactable = NetworkManager.singleton.CanStartServer();
				serverButton.onClick.SetListener(() => { NetworkManager.singleton.TryStartServer(); });
			
				quitButton.onClick.SetListener(() => { NetworkManager.Quit(); });

            	serverDropdown.interactable = NetworkManager.singleton.CanInput();
            	serverDropdown.options = NetworkManager.singleton.serverList.Select(x => new Dropdown.OptionData(x.name)).ToList();
            	NetworkManager.singleton.networkAddress = NetworkManager.singleton.serverList[serverDropdown.value].ip;
            
			}
			else Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================