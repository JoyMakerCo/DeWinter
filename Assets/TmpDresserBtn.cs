using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class TmpDresserBtn : MonoBehaviour
	{
		private Button _button;

		void Awake () {
			_button = GetComponent<Button>();
			_button.onClick.AddListener(OnClick);
		}

		void OnDestroy()
		{
			_button.onClick.RemoveListener(OnClick);
		}

		private void OnClick ()
		{
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			AmbitionApp.GetModel<GameModel>().Outfit = new OutfitVO(inventory.Inventory.Find(o=>o.Type == ItemConsts.OUTFIT));
		}
	}
}
