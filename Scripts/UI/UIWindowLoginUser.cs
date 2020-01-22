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
	// UIWindowLoginUser
	// ===================================================================================
	public partial class UIWindowLoginUser : UIRoot
	{
		
		[Header("Window")]
		public Text statusText;
		
		[Header("Input Fields")]
		public InputField usernameInput;
		public InputField userpassInput;
		
		[Header("Buttons")]
		public Button loginButton;
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
			
			if (PlayerPrefs.HasKey(Constants.PlayerPrefsUserName))
				usernameInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsUserName, "");
			if (PlayerPrefs.HasKey(Constants.PlayerPrefsPassword))
				userpassInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsPassword, "");
		}
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
		void OnDestroy()
		{
			
			if (!rememberCredentials) return;
			
			PlayerPrefs.SetString(Constants.PlayerPrefsUserName, usernameInput.text);
			PlayerPrefs.SetString(Constants.PlayerPrefsPassword, userpassInput.text);
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (!networkManager || networkManager.state != NetworkState.Offline)
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
			
			loginButton.interactable = networkManager.CanLoginUser(usernameInput.text, userpassInput.text);
			loginButton.onClick.SetListener(() => { networkManager.TryLoginUser(usernameInput.text, userpassInput.text); });
				
			cancelButton.gameObject.SetActive(networkManager.CanCancel());
			cancelButton.onClick.SetListener(() => { networkManager.TryCancel(); });
		
			backButton.onClick.SetListener(() => { Hide(); });

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================