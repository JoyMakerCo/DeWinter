using UFlow;
using System.Collections.Generic;
namespace Ambition
{
    public class NoOutfitLink : ULink
    {
        public override bool Validate() => AmbitionApp.GetModel<InventoryModel>().GetEquippedItem(ItemType.Outfit) != null;
    }
}
