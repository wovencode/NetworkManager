// =======================================================================================
// UIWindowDelete
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
	// UIWindowDelete
	// ===================================================================================
	public partial class UIWindowDelete : UIRoot
	{
	
		[Header("Network")]
		public wovencode.NetworkManager manager;
		
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField accountInput;
		public InputField accountPasswordInput;
		
		[Header("Buttons")]
		public Button deleteButton;
		public Button hostButton;
		public Button cancelButton;
		public Button backButton;
		
		[Header("Labels")]
		public string popupDescription = "Do you really want to delete this account?";
		
		protected const string _accountName = "AccountName";
		protected const string _accountPass = "AccountPass";
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
						
			if (manager.IsConnecting())
				statusText.text = "Connecting...";
			else if (!Tools.IsAllowedName(accountInput.text) || !Tools.IsAllowedPassword(accountPasswordInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			accountInput.readOnly = !manager.CanInput();
			accountPasswordInput.readOnly = !manager.CanInput();
			
			deleteButton.interactable = manager.CanDeleteAccount(accountInput.text, accountPasswordInput.text);
			deleteButton.onClick.SetListener(() => { InitPopupDelete(false); });
			
			hostButton.interactable = manager.CanDeleteAccount(accountInput.text, accountPasswordInput.text);
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
			manager.TryDeleteAccount(accountInput.text, accountPasswordInput.text, true);
		}
		
		// -------------------------------------------------------------------------------
		// onConfirmDeleteRemote
		// -------------------------------------------------------------------------------
		public void onConfirmDeleteRemote()
		{
			manager.TryDeleteAccount(accountInput.text, accountPasswordInput.text, false);
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================