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
		
		[Header("Windows")]
		public UIWindowPlayerSelect selectWindow;
		
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
		
			// -- Available Players
			UpdatePlayerPrefabs();
			UpdatePlayerIndex();
			
			// -- Buttons
			createButton.interactable = (index != -1 && !String.IsNullOrWhiteSpace(playernameInput.text));
			createButton.onClick.SetListener(() => { OnClickCreate(); });
			
			backButton.onClick.SetListener(() => { OnClickBack(); });
		
		}
		
		// -------------------------------------------------------------------------------
		// UpdatePlayerPrefabs
		// -------------------------------------------------------------------------------
		protected void UpdatePlayerPrefabs(bool forced=false)
		{
#if wPLAYER
			if (!forced && contentViewport.childCount > 0)
				return;
				
			for (int i = 0; i < contentViewport.childCount; i++)
				GameObject.Destroy(contentViewport.GetChild(i).gameObject);
			
			int _index = 0;
			
			foreach (GameObject player in networkManager.playerPrefabs)
			{

				GameObject go = GameObject.Instantiate(slotPrefab.gameObject);
				go.transform.SetParent(contentViewport.transform, false);

				go.GetComponent<UISelectPlayerSlot>().Init(buttonGroup, _index, player.name, (_index == 0) ? true : false);
				_index++;
			}
			
			index = 0;
#endif
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
		
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickCreate
		// -------------------------------------------------------------------------------
		public void OnClickCreate()
		{	
			networkManager.TryRegisterPlayer(playernameInput.text);
			//Hide();
		}
		
		// -------------------------------------------------------------------------------
		// OnClickBack
		// -------------------------------------------------------------------------------
		public void OnClickBack()
		{	
			selectWindow.Show();
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================