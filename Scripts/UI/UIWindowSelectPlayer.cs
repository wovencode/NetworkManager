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
	// UIWindowSelectPlayer
	// ===================================================================================
	public partial class UIWindowSelectPlayer : UIRoot
	{
	
		[Header("Network")]
		public wovencode.NetworkManager manager;
		
		[Header("Prefab")]
		public UISelectPlayerSlot slotPrefab;
		public UIButtonGroup buttonGroup;
		
		[Header("Content")]
		public Transform contentViewport;
		
		[Header("Buttons")]
		public Button createButton;
		public Button selectButton;
		public Button deleteButton;
		public Button backButton;
		
		protected int index = -1;
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (manager.state != NetworkState.Lobby)
			{
				Hide();
				return;
			}
			
			//createButton.interactable = 
			
			
			selectButton.interactable = (index != -1);
			
			
			deleteButton.interactable = (index != -1);
			
			
			
			backButton.onClick.SetListener(() => { Hide(); });

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================