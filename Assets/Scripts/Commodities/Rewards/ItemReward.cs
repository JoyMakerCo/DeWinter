using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class ItemReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            ItemVO item = null;

            if (inventory.Inventory.Count < inventory.NumSlots)
            {
                ItemVO[] itemz = Array.FindAll(inventory.Items, i => i.Type.ToString() == reward.ID);
                if (itemz.Length > 0)
                {
                    item = new ItemVO(itemz[Util.RNG.Generate(itemz.Length)]);
                }
                else
                {
                    Debug.LogWarningFormat("No inventory template matches reward ID '{0}'; synthesizing one", reward.ID);
                    item = new ItemVO();
                    item.Type = ItemType.Token;
                    item.ID = reward.ID;
                    item.Created = AmbitionApp.GetModel<CalendarModel>().Today;
                    item.Price = reward.Value;
                }

                if (!inventory.Inventory.ContainsKey(item.Type))
                {
                    inventory.Inventory.Add(item.Type, new List<ItemVO>());
                }
                        
                inventory.Inventory[item.Type].Add(item);
                inventory.Broadcast();
            }
        }
    }
}
