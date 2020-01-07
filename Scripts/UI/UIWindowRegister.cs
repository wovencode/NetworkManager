// =======================================================================================
// UIWindowLogin
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
	// UIWindowRegister
	// ===================================================================================
	public partial class UIWindowRegister : UIRoot
	{
	
		[Header("Network")]
		public wovencode.NetworkManager manager;
		
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField accountInput;
		public InputField passwordInput;
		
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
			else if (!Tools.IsAllowedName(accountInput.text) || !Tools.IsAllowedPassword(passwordInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			accountInput.readOnly = !manager.CanInput();
			passwordInput.readOnly = !manager.CanInput();
			
			registerButton.interactable = manager.CanRegisterAccount(accountInput.text, passwordInput.text);
			registerButton.onClick.SetListener(() => { manager.TryRegisterAccount(accountInput.text, passwordInput.text, false); });
		
			hostButton.interactable = manager.CanHostAndPlay(accountInput.text, passwordInput.text);
			hostButton.onClick.SetListener(() => { manager.TryRegisterAccount(accountInput.text, passwordInput.text, true); });
			
			cancelButton.gameObject.SetActive(manager.CanCancel());
			cancelButton.onClick.SetListener(() => { manager.TryCancel(); });
		
			backButton.onClick.SetListener(() => { Hide(); });

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================