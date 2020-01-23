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

			loginButton.interactable = networkManager.CanClick();
			loginButton.onClick.SetListener(() => { OnClickLogin(); });
			
			registerButton.interactable = networkManager.CanClick();
			registerButton.onClick.SetListener(() => { OnClickRegister(); });
			
			changePasswordButton.interactable = networkManager.CanClick();
			changePasswordButton.onClick.SetListener(() => { OnClickChangePassword(); });
			
			deleteButton.interactable = networkManager.CanClick();
			deleteButton.onClick.SetListener(() => { OnClickDeleteUser(); });
		
			serverButton.interactable = networkManager.CanStartServer();
			serverButton.onClick.SetListener(() => { OnClickStartServer(); });
		
			quitButton.onClick.SetListener(() => { OnClickQuit(); });

		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickLogin
		// -------------------------------------------------------------------------------
		public void OnClickLogin()
		{
			Hide();
			loginWindow.Show();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickRegister
		// -------------------------------------------------------------------------------
		public void OnClickRegister()
		{
			Hide();
			registerWindow.Show();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickChangePassword
		// -------------------------------------------------------------------------------
		public void OnClickChangePassword()
		{
			Hide();
			changePasswordWindow.Show();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickDeleteUser
		// -------------------------------------------------------------------------------
		public void OnClickDeleteUser()
		{
			Hide();
			deleteWindow.Show();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickStartServer
		// -------------------------------------------------------------------------------
		public void OnClickStartServer()
		{
			Hide();
			networkManager.TryStartServer();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickQuit
		// -------------------------------------------------------------------------------
		public void OnClickQuit()
		{
			Hide();
			networkManager.Quit();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================