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
			
		[Header("Network")]
		public wovencode.NetworkManager manager;
		
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
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		void Start()
		{
			if (!rememberServer) return;
			
			if (PlayerPrefs.HasKey(Constants.PlayerPrefsLastServer))
			{
				string lastServer = PlayerPrefs.GetString(Constants.PlayerPrefsLastServer, "");
				serverDropdown.value = manager.serverList.FindIndex(s => s.name == lastServer);
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
			
			if (manager.state == NetworkState.Offline)
			{
				Show();
				
				loginButton.interactable = manager.CanClick();
				loginButton.onClick.SetListener(() => { loginWindow.Show(); });
				
				registerButton.interactable = manager.CanClick();
				registerButton.onClick.SetListener(() => { registerWindow.Show(); });
				
				changePasswordButton.interactable = manager.CanClick();
				changePasswordButton.onClick.SetListener(() => { changePasswordWindow.Show(); });
				
				deleteButton.interactable = manager.CanClick();
				deleteButton.onClick.SetListener(() => { deleteWindow.Show(); });
			
				serverButton.interactable = manager.CanStartServer();
				serverButton.onClick.SetListener(() => { manager.TryStartServer(); });
			
				quitButton.onClick.SetListener(() => { wovencode.NetworkManager.Quit(); });

            	serverDropdown.interactable = manager.CanInput();
            	serverDropdown.options = manager.serverList.Select(x => new Dropdown.OptionData(x.name)).ToList();
            	manager.networkAddress = manager.serverList[serverDropdown.value].ip;
            
			}
			else Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================