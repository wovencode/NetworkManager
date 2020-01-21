// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using UnityEngine;
using UnityEngine.UI;
using Wovencode;
using Wovencode.Network;
using Wovencode.UI;

namespace Wovencode.UI
{
	
	// ===================================================================================
	// UISelectPlayerSlot
	// ===================================================================================
	public partial class UISelectPlayerSlot : UIButton
	{
		
		[Header("UI Elements")]
		public Text textName;
		public Image imageSelected;
		public Button buttonSelect;
		
		[Header("Used Images")]
		public Sprite unselectedImage;
		public Sprite selectedImage;
		
		protected int _index;
		protected bool selected;
		
		// -------------------------------------------------------------------------------
		// Init
		// -------------------------------------------------------------------------------
		public void Init(UIButtonGroup _buttonGroup, int index, string name, bool _selected=false)
		{
			
			selected = _selected;
			
			base.Init(_buttonGroup);
			
			_index = index;
			textName.text = name;
			
		}
		
		// -------------------------------------------------------------------------------
		// OnPressed
		// -------------------------------------------------------------------------------
		public override void OnPressed()
		{
		
			if (selected)
			{
				selected = false;
				imageSelected.sprite = unselectedImage;
				
			}
			else
			{
				selected = true;
				imageSelected.sprite = selectedImage;
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// Index
		// -------------------------------------------------------------------------------
		public int Index
		{
			get { return (selected) ? _index : -1; }
		}
				
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================