// =======================================================================================
// UIWindowMain
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
		public UIWindowLogin loginWindow;
		public UIWindowRegister registerWindow;
		public UIWindowDelete deleteWindow;
		
		[Header("Buttons")]
		public Button loginButton;
		public Button registerButton;
		public Button deleteButton;
		public Button serverButton;
		public Button quitButton;
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (manager.state != NetworkState.Game)
			{
				Show();
				
				loginButton.interactable = manager.CanClick();
				loginButton.onClick.SetListener(() => { loginWindow.Show(); });
				
				registerButton.interactable = manager.CanClick();
				registerButton.onClick.SetListener(() => { registerWindow.Show(); });
				
				deleteButton.interactable = manager.CanClick();
				deleteButton.onClick.SetListener(() => { deleteWindow.Show(); });
			
				serverButton.interactable = manager.CanStartServer();
				serverButton.onClick.SetListener(() => { manager.TryStartServer(); });
			
				quitButton.onClick.SetListener(() => { wovencode.NetworkManager.Quit(); });


			}
			else Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================