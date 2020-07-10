using System;
using System.Collections.Generic;
using Core;
namespace Ambition
{
    public class EquipItemCmd : ICommand<ItemVO>
    {
        public void Execute(ItemVO item)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            inventory.Inventory.Remove(item);
            if (inventory.Equipped.TryGetValue(item.Type, out ItemVO equipped))
            {
                equipped.Equipped = false;
            }
            item.Equipped = true;
            inventory.Equipped[item.Type] = item;
            inventory.Broadcast();
            AmbitionApp.GetModel<LocalizationModel>().SetPartyOutfit(item);
        }
    }
}
