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
	// UIWindowChangePasswordUser
	// ===================================================================================
	public partial class UIWindowChangePasswordUser : UIRoot
	{
	
		[Header("Network")]
		public wovencode.NetworkManager manager;
		
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField oldUserPassInput;
		public InputField newUserPassInput;
		
		[Header("Buttons")]
		public Button changeButton;
		public Button cancelButton;
		public Button backButton;
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (manager.state == NetworkState.Game)
			{
				Hide();
				return;
			}
			
			if (manager.IsConnecting())
				statusText.text = "Connecting...";
			else if (!Tools.IsAllowedName(usernameInput.text) || !Tools.IsAllowedPassword(oldUserPassInput.text) || !Tools.IsAllowedPassword(newUserPassInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			usernameInput.readOnly = !manager.CanInput();
			oldUserPassInput.readOnly = !manager.CanInput();
			newUserPassInput.readOnly = !manager.CanInput();
			
			changeButton.interactable = manager.CanChangePasswordUser(usernameInput.text, oldUserPassInput.text, newUserPassInput.text);
			changeButton.onClick.SetListener(() => { manager.TryChangePasswordUser(usernameInput.text, oldUserPassInput.text, newUserPassInput.text); });
		
			cancelButton.gameObject.SetActive(manager.CanCancel());
			cancelButton.onClick.SetListener(() => { manager.TryCancel(); });
		
			backButton.onClick.SetListener(() => { Hide(); });

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================