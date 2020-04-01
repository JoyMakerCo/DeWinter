using Core;
using System.Collections.Generic;
namespace Ambition
{
    public class DeleteItemCmd : ICommand<ItemVO>
    {
        public void Execute(ItemVO item)
        {
            AmbitionApp.SendMessage(InventoryMessages.UNEQUIP, item);
            if (AmbitionApp.GetModel<InventoryModel>().Inventory.TryGetValue(item.Type, out List<ItemVO> items) && items.Remove(item))
            {
                AmbitionApp.SendMessage(InventoryMessages.ITEM_DELETED, item);
            }
        }
    }
}
