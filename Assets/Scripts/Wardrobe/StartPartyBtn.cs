﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class StartPartyBtn : MonoBehaviour
	{
		private Button _button;

		void Awake()
		{
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            _button = gameObject.GetComponent<Button>();
            _button.interactable = false;
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleEquip);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.UNEQUIP, HandleUnequip);
            HandleEquip(inventory.GetEquippedItem(ItemType.Outfit));
		}

		void OnDestroy ()
		{
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleEquip);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.UNEQUIP, HandleUnequip);
        }

        private void HandleEquip(ItemVO item)
        {
            _button.interactable = item?.Type == ItemType.Outfit;
        }

        private void HandleUnequip(ItemVO item)
        {
            if (item?.Type == ItemType.Outfit)
                _button.interactable = false;
        }
    }
}
