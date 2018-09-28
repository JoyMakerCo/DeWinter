using System.Collections.Generic;
using UFlow;

namespace Ambition
{
    public class InitPartyGearState : UState
    {
        public override void OnEnterState()
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            List<ItemVO> items = inventory.Inventory.FindAll(i => i.Type == ItemConsts.ACCESSORY);
            OutfitVO outfit=null;
            if (items.Count == 0)
            {
                items = inventory.Inventory.FindAll(i => i.Type == ItemConsts.OUTFIT);
                if (items.Count == 1) outfit = new OutfitVO(items[0]);
            }
            inventory.Equipped[ItemConsts.OUTFIT] = outfit;
            inventory.Equipped[ItemConsts.ACCESSORY] = null;
        }
    }
}
