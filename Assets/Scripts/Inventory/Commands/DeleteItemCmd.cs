using Core;
using System.Collections.Generic;
namespace Ambition
{
    public class DeleteItemCmd : ICommand<ItemVO>
    {
        public void Execute(ItemVO item)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            inventory.Equipped.Remove(item.Type);
            inventory.Inventory.Remove(item);
            AmbitionApp.SendMessage(InventoryMessages.ITEM_DELETED, item);
            inventory.Broadcast();
        }
    }
}
