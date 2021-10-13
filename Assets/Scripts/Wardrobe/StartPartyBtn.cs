using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class StartPartyBtn : MonoBehaviour
	{
		public Button Button;

		void Awake()
		{
            AmbitionApp.Inventory.Observe(HandleInventory);
		}

		void OnDestroy ()
		{
            AmbitionApp.Inventory.Unobserve(HandleInventory);
        }

        private void HandleInventory(InventoryModel inventory)
        {
            Button.interactable = inventory.GetItems(ItemType.Outfit)?.Length > 0;
        }
    }
}
