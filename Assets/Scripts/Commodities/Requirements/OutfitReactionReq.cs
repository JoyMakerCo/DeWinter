using System;
namespace Ambition
{
    public static class OutfitReactionReq
    {
        public static bool Check(RequirementVO req)
        {
            var party = AmbitionApp.GetModel<PartyModel>().Party;

			// outfit reaction credibility shift
            ItemVO outfit = AmbitionApp.Inventory.GetEquippedItem(ItemType.Outfit);
            CalendarEvent e = AmbitionApp.GetEvent();
            FactionType faction;
            if (e is PartyVO) faction = ((PartyVO)e).Faction;
            else
            {
                CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(((RendezVO)e).Character);
                faction = character?.Faction ?? FactionType.Military;
            }
			int value = AmbitionApp.Inventory.GetFactionBonus(outfit, faction);
            return RequirementsSvc.Check(req, value);
        }
    }
}
