using Core;
using System.Collections.Generic;
namespace Ambition
{
    public class DeleteItemCmd : ICommand<ItemVO>
    {
        public void Execute(ItemVO item)
        {
            AmbitionApp.Inventory.Inventory.Remove(item);
            AmbitionApp.SendMessage(InventoryMessages.ITEM_DELETED, item);
            AmbitionApp.Inventory.Broadcast();
        }
    }
}
