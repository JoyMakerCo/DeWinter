using System;
namespace Ambition
{
    public class ItemReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            if (inventory.Inventory.Count < inventory.NumSlots)
            {
                ItemVO[] itemz = Array.FindAll(inventory.ItemDefinitions, i => i.Type == reward.ID);
                ItemVO item = new ItemVO(itemz[Util.RNG.Generate(0, itemz.Length)])
                {
                    Quantity = reward.Value
                };
                inventory.Inventory.Add(item);
            }
        }
    }
}
