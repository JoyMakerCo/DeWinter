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
            ItemVO item = inventory.Instantiate(reward.ID);
            if (item == null)
            {
                ItemConfig config = Resources.Load<ItemConfig>(Filepath.Items + reward.ID);
                inventory.Import(config);
                item = inventory.Instantiate(reward.ID);
            }
            if (item != null)
            {
                if (reward.Value > 0) item.Price = reward.Value;
                item.Created = AmbitionApp.GetModel<CalendarModel>().Today;
                inventory.Inventory.Add(item);
                inventory.Broadcast();
            }
#if DEBUG
            else Debug.LogWarningFormat("No inventory template matches reward ID '{0}'; synthesizing one", reward.ID);
#endif
        }
    }
}
