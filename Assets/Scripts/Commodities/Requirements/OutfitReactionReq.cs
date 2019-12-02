using System;
namespace Ambition
{
    public static class OutfitReactionReq
    {
        public static bool Check(RequirementVO req)
        {
            var party = AmbitionApp.GetModel<PartyModel>().Party;

			// outfit reaction credibility shift
            var outfit = AmbitionApp.GetModel<InventoryModel>().GetEquippedItem(ItemType.Outfit);
			int value = AmbitionApp.GetModel<InventoryModel>().GetCredibilityBonus(outfit,party.Faction);
            return RequirementsSvc.Check(req, value);
        }
    }
}
