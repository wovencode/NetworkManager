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
		public bool rememberCredentials;
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		void Start()
		{
			if (!rememberCredentials) return;
			
			if (PlayerPrefs.HasKey(Constants.PP_USERNAME))
				accountInput.text = PlayerPrefs.GetString(Constants.PP_USERNAME, "");
			if (PlayerPrefs.HasKey(Constants.PP_PASSWORD))
				accountPasswordInput.text = PlayerPrefs.GetString(Constants.PP_PASSWORD, "");
		}
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
		void OnDestroy()
		{
			
			if (!rememberCredentials) return;
			
			PlayerPrefs.SetString(Constants.PP_USERNAME, accountInput.text);
			PlayerPrefs.SetString(Constants.PP_PASSWORD, accountPasswordInput.text);
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