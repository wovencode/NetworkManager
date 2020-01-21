// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using Wovencode.UI;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Wovencode.UI
{

	// ===================================================================================
	// UIWindowPlayerCreate
	// ===================================================================================
	public partial class UIWindowPlayerCreate : UIRoot
	{
	
		[Header("Prefab")]
		public UISelectPlayerSlot slotPrefab;
		public UIButtonGroup buttonGroup;
		
		[Header("Content")]
		public Transform contentViewport;
		
		[Header("Input Fields")]
		public InputField playernameInput;
		
		[Header("Buttons")]
		public Button createButton;
		public Button backButton;
		
		protected int index = -1;
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (!networkManager || networkManager.state != NetworkState.Lobby)
			{
				Hide();
				return;
			}
			else if (root.activeSelf)
			{
			
				createButton.interactable = (!String.IsNullOrWhiteSpace(playernameInput.text));
				createButton.onClick.SetListener(() => { networkManager.TryRegisterPlayer(playernameInput.text); });
				
				backButton.onClick.SetListener(() => { Hide(); });
			
			
			}
			
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================