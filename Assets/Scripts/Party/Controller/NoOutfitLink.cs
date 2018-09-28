using UFlow;

namespace Ambition
{
    public class NoOutfitLink : ULink
    {
        public override bool Validate()
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            ItemVO item;
            return !inventory.Equipped.TryGetValue(ItemConsts.OUTFIT, out item) || item == null;
        }
    }
}
