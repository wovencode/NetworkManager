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
			
			if (NetworkManager.singleton.state != NetworkState.Offline)
			{
				Hide();
				return;
			}
			
			if (NetworkManager.singleton.IsConnecting())
				statusText.text = "Connecting...";
			else if (!Tools.IsAllowedName(usernameInput.text) || !Tools.IsAllowedPassword(userpassInput.text))
				statusText.text = "Check Name/Password";
			else
				statusText.text = "";
			
			loginButton.interactable = NetworkManager.singleton.CanLoginUser(usernameInput.text, userpassInput.text);
			loginButton.onClick.SetListener(() => { NetworkManager.singleton.TryLoginUser(usernameInput.text, userpassInput.text); });
				
			cancelButton.gameObject.SetActive(NetworkManager.singleton.CanCancel());
			cancelButton.onClick.SetListener(() => { NetworkManager.singleton.TryCancel(); });
		
			backButton.onClick.SetListener(() => { Hide(); });

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================