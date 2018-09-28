using Core;
namespace Ambition
{
    public class DeleteItemCommand : ICommand<ItemVO>
    {
        public void Execute(ItemVO item)
        {
            AmbitionApp.SendMessage(InventoryMessages.UNEQUIP, item);
            if (AmbitionApp.GetModel<InventoryModel>().Inventory.Remove(item))
            {
                AmbitionApp.SendMessage(InventoryMessages.ITEM_DELETED, item);
            }
        }
    }
}
