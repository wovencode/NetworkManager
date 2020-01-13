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
	// UIWindowRegisterUser
	// ===================================================================================
	public partial class UIWindowRegisterUser : UIRoot
	{
	
		[Header("Network")]
		public wovencode.NetworkManager manager;
		
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField userpassInput;
		
		[Header("Buttons")]
		public Button registerButton;
		public Button hostButton;
		public Button cancelButton;
		public Button backButton;
		
		protected const string _accountName = "AccountName";
		protected const string _accountPass = "AccountPass";
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{

			if (manager.IsConnecting())
				statusText.text = "Connecting...";
			else if (!Tools.IsAllowedName(usernameInput.text) || !Tools.IsAllowedPassword(userpassInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			usernameInput.readOnly = !manager.CanInput();
			userpassInput.readOnly = !manager.CanInput();
			
			registerButton.interactable = manager.CanRegisterUser(usernameInput.text, userpassInput.text);
			registerButton.onClick.SetListener(() => { manager.TryRegisterUser(usernameInput.text, userpassInput.text, false); });
		
			hostButton.interactable = manager.CanHostAndPlay(usernameInput.text, userpassInput.text);
			hostButton.onClick.SetListener(() => { manager.TryRegisterUser(usernameInput.text, userpassInput.text, true); });
			
			cancelButton.gameObject.SetActive(manager.CanCancel());
			cancelButton.onClick.SetListener(() => { manager.TryCancel(); });
		
			backButton.onClick.SetListener(() => { Hide(); });

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================