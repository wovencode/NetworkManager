// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using Wovencode.UI;
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
		
		[Header("Buttons")]
		public Button loginButton;
		public Button registerButton;
		public Button changePasswordButton;
		public Button deleteButton;
		public Button serverButton;
		public Button quitButton;
		
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
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{

			if (networkManager && networkManager.isNetworkActive && networkManager.state == NetworkState.Offline)
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

			}
			else Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================