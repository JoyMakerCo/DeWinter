using UFlow;

namespace Ambition
{
    public class CheckOutfitLink : ULink
    {
        public override bool Validate()
        {
            return AmbitionApp.GetModel<InventoryModel>().GetEquipped(ItemConsts.OUTFIT) is OutfitVO;
        }
    }
}
