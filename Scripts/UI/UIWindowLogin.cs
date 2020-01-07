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
	// UIWindowLogin
	// ===================================================================================
	public partial class UIWindowLogin : UIRoot
	{
	
		[Header("Network")]
		public wovencode.NetworkManager manager;
		
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField accountInput;
		public InputField accountPasswordInput;
		
		[Header("Buttons")]
		public Button loginButton;
		public Button hostButton;
		public Button cancelButton;
		public Button backButton;
		
		[Header("Settings")]
		public bool rememberData;
		
		protected const string _accountName = "AccountName";
		protected const string _accountPass = "AccountPass";
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		void Start()
		{
			if (!rememberData) return;
			
			if (PlayerPrefs.HasKey(_accountName))
				accountInput.text = PlayerPrefs.GetString(_accountName, "");
			if (PlayerPrefs.HasKey(_accountPass))
				accountPasswordInput.text = PlayerPrefs.GetString(_accountPass, "");
		}
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
		void OnDestroy()
		{
			
			if (!rememberData) return;
			
			PlayerPrefs.SetString(_accountName, accountInput.text);
			PlayerPrefs.SetString(_accountPass, accountPasswordInput.text);
		}
		
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
			else if (!Tools.IsAllowedName(accountInput.text) || !Tools.IsAllowedPassword(accountPasswordInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			accountInput.readOnly = !manager.CanInput();
			accountPasswordInput.readOnly = !manager.CanInput();
			
			loginButton.interactable = manager.CanLoginAccount(accountInput.text, accountPasswordInput.text);
			loginButton.onClick.SetListener(() => { manager.TryLoginAccount(accountInput.text, accountPasswordInput.text, false); });
		
			hostButton.interactable = manager.CanHostAndPlay(accountInput.text, accountPasswordInput.text);
			hostButton.onClick.SetListener(() => { manager.TryLoginAccount(accountInput.text, accountPasswordInput.text, true); });
		
			cancelButton.gameObject.SetActive(manager.CanCancel());
			cancelButton.onClick.SetListener(() => { manager.TryCancel(); });
		
			backButton.onClick.SetListener(() => { Hide(); });

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================