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
			
			deleteButton.interactable = networkManager.CanDeleteUser(usernameInput.text, userpassInput.text);
			deleteButton.onClick.SetListener(() => { InitPopupDelete(); });
					
			cancelButton.gameObject.SetActive(networkManager.CanCancel());
			cancelButton.onClick.SetListener(() => { networkManager.TryCancel(); });
			
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
			networkManager.TryDeleteUser(usernameInput.text, userpassInput.text);
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================