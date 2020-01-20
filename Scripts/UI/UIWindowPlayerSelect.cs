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
	// UIWindowPlayerSelect
	// ===================================================================================
	public partial class UIWindowPlayerSelect : UIRoot
	{
		
		[Header("Windows")]
		public UIWindowPlayerCreate createWindow;
		
		[Header("Prefab")]
		public UISelectPlayerSlot slotPrefab;
		public UIButtonGroup buttonGroup;
		
		[Header("Content")]
		public Text textMaxPlayers;
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
			
			if (NetworkManager.singleton.state != NetworkState.Lobby)
			{
				Hide();
				return;
			}
			else
			{
			
				//createButton.interactable = 
				createButton.onClick.SetListener(() => { createWindow.Show(); });
			
				selectButton.interactable = (index != -1);
			
			
				deleteButton.interactable = (index != -1);
						
						
				backButton.onClick.SetListener(() => { Hide(); });
			
			
				Show();
			}
			
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================