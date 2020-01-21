// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using Wovencode.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Wovencode.UI
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
			
			if (!rememberServer) return;
			
			if (PlayerPrefs.HasKey(Constants.PlayerPrefsLastServer))
			{
				string lastServer = PlayerPrefs.GetString(Constants.PlayerPrefsLastServer, "");
				serverDropdown.value = networkManager.serverList.FindIndex(s => s.name == lastServer);
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

			if (networkManager && networkManager.state == NetworkState.Offline)
			{
				
				loginButton.interactable = networkManager.CanClick();
				loginButton.onClick.SetListener(() => { loginWindow.Show(); });
				
				registerButton.interactable = networkManager.CanClick();
				registerButton.onClick.SetListener(() => { registerWindow.Show(); });
				
				changePasswordButton.interactable = networkManager.CanClick();
				changePasswordButton.onClick.SetListener(() => { changePasswordWindow.Show(); });
				
				deleteButton.interactable = networkManager.CanClick();
				deleteButton.onClick.SetListener(() => { deleteWindow.Show(); });
			
				serverButton.interactable = networkManager.CanStartServer();
				serverButton.onClick.SetListener(() => { networkManager.TryStartServer(); });
			
				quitButton.onClick.SetListener(() => { networkManager.Quit(); });

            	serverDropdown.interactable = networkManager.CanClick();
            	serverDropdown.options = networkManager.serverList.Select(x => new Dropdown.OptionData(x.name)).ToList();
            	networkManager.networkAddress = networkManager.serverList[serverDropdown.value].ip;
            
			}
			else Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================