using System;

namespace Ambition
{
    public class ApplyOutfitEffectCmd : Core.ICommand
    {
        public void Execute()
        {
            PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
            OutfitVO outfit = AmbitionApp.Inventory.GetEquippedItem(ItemType.Outfit) as OutfitVO;
            AmbitionApp.GetModel<FactionModel>().Factions.TryGetValue(party.Faction, out FactionVO faction);
            int score = 200 - Math.Abs(faction.Modesty - outfit.Modesty) - Math.Abs(faction.Luxury - outfit.Luxury);
            int cred = (int)(score * outfit.Novelty * .001f);
            CommodityVO reward = new CommodityVO(CommodityType.Credibility, cred);
            AmbitionApp.SendMessage(reward);
        }
    }
}
