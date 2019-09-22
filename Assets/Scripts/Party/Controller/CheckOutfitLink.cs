using UFlow;
using System.Collections.Generic;
namespace Ambition
{
    public class CheckOutfitLink : ULink
    {
        public override bool Validate() => AmbitionApp.GetModel<InventoryModel>().GetEquippedItem(ItemType.Outfit) != null;
    }
}
