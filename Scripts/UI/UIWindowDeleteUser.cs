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
	// UIWindowDeleteUser
	// ===================================================================================
	public partial class UIWindowDeleteUser : UIRoot
	{
	
		[Header("Network")]
		public wovencode.NetworkManager manager;
		
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField userpassInput;
		
		[Header("Buttons")]
		public Button deleteButton;
		public Button hostButton;
		public Button cancelButton;
		public Button backButton;
		
		[Header("Labels")]
		public string popupDescription = "Do you really want to delete this account?";
		
		protected const string _userName = "UserName";
		protected const string _userPass = "UserPass";
		
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
			
			deleteButton.interactable = manager.CanDeleteUser(usernameInput.text, userpassInput.text);
			deleteButton.onClick.SetListener(() => { InitPopupDelete(false); });
			
			hostButton.interactable = manager.CanDeleteUser(usernameInput.text, userpassInput.text);
			hostButton.onClick.SetListener(() => { InitPopupDelete(true); });
		
			cancelButton.gameObject.SetActive(manager.CanCancel());
			cancelButton.onClick.SetListener(() => { manager.TryCancel(); });
			
			backButton.onClick.SetListener(() => { Hide(); });
				
		}
		
		// -------------------------------------------------------------------------------
		// InitPopupDelete
		// -------------------------------------------------------------------------------
		protected void InitPopupDelete(bool hostMode=false)
		{
			//TODO: Improve
			if (hostMode)
				UIPopupPrompt.singleton.Init(popupDescription, "", "", onConfirmDeleteLocal);
			else
				UIPopupPrompt.singleton.Init(popupDescription, "", "", onConfirmDeleteRemote);
		}
		
		// -------------------------------------------------------------------------------
		// onConfirmDeleteLocal
		// -------------------------------------------------------------------------------
		public void onConfirmDeleteLocal()
		{
			manager.TryDeleteUser(usernameInput.text, userpassInput.text, true);
		}
		
		// -------------------------------------------------------------------------------
		// onConfirmDeleteRemote
		// -------------------------------------------------------------------------------
		public void onConfirmDeleteRemote()
		{
			manager.TryDeleteUser(usernameInput.text, userpassInput.text, false);
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================