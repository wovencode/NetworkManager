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
	// UIWindowChangePasswordUser
	// ===================================================================================
	public partial class UIWindowChangePasswordUser : UIRoot
	{
	
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
			
			if (!networkManager || networkManager.state == NetworkState.Game)
			{
				Hide();
				return;
			}
			
			if (networkManager.IsConnecting())
				statusText.text = "Connecting...";
			else if (!Tools.IsAllowedName(usernameInput.text) || !Tools.IsAllowedPassword(oldUserPassInput.text) || !Tools.IsAllowedPassword(newUserPassInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			changeButton.interactable = networkManager.CanChangePasswordUser(usernameInput.text, oldUserPassInput.text, newUserPassInput.text);
			changeButton.onClick.SetListener(() => { networkManager.TryChangePasswordUser(usernameInput.text, oldUserPassInput.text, newUserPassInput.text); });
		
			cancelButton.gameObject.SetActive(networkManager.CanCancel());
			cancelButton.onClick.SetListener(() => { networkManager.TryCancel(); });
		
			backButton.onClick.SetListener(() => { Hide(); });

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================