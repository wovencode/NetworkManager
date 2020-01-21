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
		//public Button backButton;
		
		protected int index = -1;
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (!networkManager || networkManager.state != NetworkState.Lobby)
			{
				Hide();
				return;
			}
			else
			{
				
				// -- Max Players Test
				textMaxPlayers.text = networkManager.playerPreviews.Count.ToString() + " / " + networkManager.maxPlayers.ToString();
				
				// -- Available Players
				UpdatePlayerPreviews();
				UpdatePlayerIndex();
				
				// -- Buttons
				
				createButton.interactable = networkManager.playerPreviews.Count < networkManager.maxPlayers;
				createButton.onClick.SetListener(() => { createWindow.Show(); });
			
				selectButton.interactable = (index != -1);
			
			
				deleteButton.interactable = (index != -1);
						
						
				//backButton.onClick.SetListener(() => { Hide(); });
			
			
				Show();
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// UpdatePlayerPreviews
		// -------------------------------------------------------------------------------
		protected void UpdatePlayerPreviews(bool forced=false)
		{
			
			if (!forced && contentViewport.childCount > 0)
				return;
				
			for (int i = 0; i < contentViewport.childCount; i++)
				GameObject.Destroy(contentViewport.GetChild(i).gameObject);
			
			int _index = 0;
			
			foreach (PlayerPreview player in networkManager.playerPreviews)
			{
			
				GameObject go = GameObject.Instantiate(slotPrefab.gameObject);
				go.transform.SetParent(contentViewport.transform, false);

				go.GetComponent<UISelectPlayerSlot>().Init(buttonGroup, _index, player.name, (_index == 0) ? true : false);
				_index++;
			}
			
			index = 0;
		
		}
		
		// -------------------------------------------------------------------------------
		// UpdatePlayerIndex
		// -------------------------------------------------------------------------------
		protected void UpdatePlayerIndex()
		{
			
			foreach (UIButton button in buttonGroup.buttons)
			{
				int _index = button.GetComponent<UISelectPlayerSlot>().Index;
				if (_index != -1)
				{
					index = _index;
					return;
				}
			}
			
			index = -1;
			
		}
				
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================