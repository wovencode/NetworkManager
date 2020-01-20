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
	
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField userpassInput;
		
		[Header("Buttons")]
		public Button deleteButton;
		public Button cancelButton;
		public Button backButton;
		
		[Header("System Texts")]
		public string popupDescription = "Do you really want to delete this account?";
		
		protected const string _userName = "UserName";
		protected const string _userPass = "UserPass";
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
						
			if (NetworkManager.singleton.IsConnecting())
				statusText.text = "Connecting...";
			else if (!Tools.IsAllowedName(usernameInput.text) || !Tools.IsAllowedPassword(userpassInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			deleteButton.interactable = NetworkManager.singleton.CanDeleteUser(usernameInput.text, userpassInput.text);
			deleteButton.onClick.SetListener(() => { InitPopupDelete(); });
					
			cancelButton.gameObject.SetActive(NetworkManager.singleton.CanCancel());
			cancelButton.onClick.SetListener(() => { NetworkManager.singleton.TryCancel(); });
			
			backButton.onClick.SetListener(() => { Hide(); });
				
		}
		
		// -------------------------------------------------------------------------------
		// InitPopupDelete
		// -------------------------------------------------------------------------------
		protected void InitPopupDelete()
		{
			UIPopupPrompt.singleton.Init(popupDescription, "", "", onConfirmDelete);
		}
		
		// -------------------------------------------------------------------------------
		// onConfirmDelete
		// -------------------------------------------------------------------------------
		public void onConfirmDelete()
		{
			NetworkManager.singleton.TryDeleteUser(usernameInput.text, userpassInput.text);
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================