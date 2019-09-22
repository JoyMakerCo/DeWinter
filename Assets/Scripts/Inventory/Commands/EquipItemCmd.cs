using System;
using System.Collections.Generic;
using Core;
namespace Ambition
{
    public class EquipItemCmd : ICommand<ItemVO>
    {
        public void Execute(ItemVO item) => AmbitionApp.GetModel<InventoryModel>().Equip(item);
    }
}
