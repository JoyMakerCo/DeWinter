using System;
using System.Collections.Generic;
using Core;
namespace Ambition
{
    public class EquipItemCmd : ICommand<ItemVO>
    {
        public void Execute(ItemVO item)
        {
            if (item == null) return;
            AmbitionApp.Inventory.Inventory.ForEach(i => { if (i.Type == item.Type) i.Equipped = false; });
            item.Equipped = true;
            if (!AmbitionApp.Inventory.Inventory.Contains(item))
                AmbitionApp.Inventory.Inventory.Add(item);
            if (item is OutfitVO)
            {
                AmbitionApp.GetModel<LocalizationModel>().SetPartyOutfit((OutfitVO)item);
            }
            AmbitionApp.Inventory.Broadcast();
        }
    }
}
