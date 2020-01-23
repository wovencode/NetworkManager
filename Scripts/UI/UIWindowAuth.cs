// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using Wovencode.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Wovencode.UI
{

	// ===================================================================================
	// UIWindowAuth
	// ===================================================================================
	public partial class UIWindowAuth : UIRoot
	{
		
		[Header("Settings")]
		public bool rememberServer;
		
		[Header("Dropdown")]
		public Dropdown serverDropdown;
		
		[Header("Buttons")]
		public Button connectButton;
		public Text connectButtonText;
		
		[Header("System Texts")]
		public UIWindowAuth_Lang systemTexts;
		
		public static UIWindowAuth singleton;
		
		protected int connectTimer = -1;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
			
#if !_SERVER
				Hide();
				return;
#endif
			
			serverDropdown.options = networkManager.serverList.Select(x => new Dropdown.OptionData(x.name)).ToList();
			
			if (rememberServer && PlayerPrefs.HasKey(Constants.PlayerPrefsLastServer))
			{
				string lastServer = PlayerPrefs.GetString(Constants.PlayerPrefsLastServer, "");
				serverDropdown.value = networkManager.serverList.FindIndex(s => s.name == lastServer);
			}
			
			networkManager.networkAddress = networkManager.serverList[serverDropdown.value].ip;
			
		}
		
		// -------------------------------------------------------------------------------
		// Show
		// -------------------------------------------------------------------------------
		public override void Show()
		{
			base.Show();
			
			connectTimer = -1;
			
			if (networkAuthenticator.connectTimeout > 0)
				Invoke(nameof(Timeout), networkAuthenticator.connectTimeout);
			
		}
		
		// -------------------------------------------------------------------------------
		// Hide
		// -------------------------------------------------------------------------------
		public override void Hide()
		{
			
			CancelInvoke();
			
			if (rememberServer)
				PlayerPrefs.SetString(Constants.PlayerPrefsLastServer, serverDropdown.captionText.text);
		
			base.Hide();
			
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (connectTimer == -1)
				connectTimer = networkAuthenticator.connectDelay;
				
			if (networkManager.IsConnecting())
			{
			
				if (connectButtonText)
				{
					connectTimer--;
					connectButtonText.text = systemTexts.clientConnect + " (in " + connectTimer.ToString() + "s)";
				}
			
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// Timeout
		// -------------------------------------------------------------------------------
		protected void Timeout()
		{
			UIPopupConfirm.singleton.Init(systemTexts.serverOffline, "", OnClickQuit);
		}
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// onClickQuit
		// -------------------------------------------------------------------------------
		protected void OnClickQuit()
		{
			networkManager.StopClient();
			networkManager.Quit();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickConnect
		// -------------------------------------------------------------------------------
		public void OnClickConnect()
		{
			CancelInvoke();
			networkAuthenticator.ClientAuthenticate();
		}
		
		// -------------------------------------------------------------------------------
		// OnDropdownChange
		// -------------------------------------------------------------------------------
		public void OnDropdownChange()
		{
			if (rememberServer)
				PlayerPrefs.SetString(Constants.PlayerPrefsLastServer, serverDropdown.captionText.text);
			
            networkManager.networkAddress = networkManager.serverList[serverDropdown.value].ip;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================