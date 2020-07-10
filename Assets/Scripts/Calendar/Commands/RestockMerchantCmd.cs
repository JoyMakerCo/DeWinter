using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class RestockMerchantCmd : ICommand
	{
		public void Execute()
		{
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            // Ensure that the same market values persist throughout the day
            if (inventory.Market != null) return;

            FactionVO bonus = AmbitionApp.GetModel<FactionModel>()[FactionType.Bourgeoisie];
            int count = inventory.NumMarketSlots + (bonus?.Level >= 3 ? 1 : 0);
            ItemVO item;
            inventory.Market = new List<ItemVO>();
			for (int i=0; i<count; i++)
			{
                item = new ItemVO();
                AmbitionApp.SendMessage(InventoryMessages.GENERATE_OUTFIT, item);
                inventory.Market.Add(item);
			}
        }
    }
}
	