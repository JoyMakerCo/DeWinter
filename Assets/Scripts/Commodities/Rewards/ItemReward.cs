using System;
using System.Collections.Generic;
namespace Ambition
{
    public class ItemReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            if (inventory.Inventory.Count < inventory.NumSlots)
            {
                ItemVO[] itemz = Array.FindAll(inventory.Items, i => i.Type.ToString() == reward.ID);
                if (itemz.Length > 0)
                {
                    ItemVO item = new ItemVO(itemz[Util.RNG.Generate(itemz.Length)]);
                    if (!inventory.Inventory.ContainsKey(item.Type))
                        inventory.Inventory.Add(item.Type, new List<ItemVO>());
                    inventory.Inventory[item.Type].Add(item);
                }
            }
        }
    }
}
