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
	// UIWindowRegisterUser
	// ===================================================================================
	public partial class UIWindowRegisterUser : UIRoot
	{
	
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField userpassInput;
		public InputField usermailInput;
		
		[Header("Buttons")]
		public Button registerButton;
		public Button cancelButton;
		public Button backButton;
		
		protected const string _accountName = "AccountName";
		protected const string _accountPass = "AccountPass";
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (!networkManager)
			{
				Hide();
				return;
			}
				
			if (networkManager.IsConnecting())
				statusText.text = "Connecting...";
			else if (!Tools.IsAllowedName(usernameInput.text) || !Tools.IsAllowedPassword(userpassInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			registerButton.interactable = networkManager.CanRegisterUser(usernameInput.text, userpassInput.text);
			registerButton.onClick.SetListener(() => { networkManager.TryRegisterUser(usernameInput.text, userpassInput.text, usermailInput.text); });
					
			cancelButton.gameObject.SetActive(networkManager.CanCancel());
			cancelButton.onClick.SetListener(() => { networkManager.TryCancel(); });
		
			backButton.onClick.SetListener(() => { Hide(); });

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================