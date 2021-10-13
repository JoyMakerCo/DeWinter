using System;
using System.Collections.Generic;
namespace Ambition
{
    public class ExitAfterPartyState : UFlow.UState
    {
        public override void OnEnter()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            OutfitVO outfit;
            foreach(ItemVO item in AmbitionApp.Inventory.Inventory)
            {
                outfit = item as OutfitVO;
                if (outfit != null)
                {
                    if (outfit.Equipped)
                    {
                        outfit.Novelty -= model.BaseNoveltyLoss + outfit.TimesWorn * model.CumulativeNoveltyLoss;
                        if (outfit.Novelty < 0) outfit.Novelty = 0;
                        ++outfit.TimesWorn;
                    }
                    else outfit.TimesWorn = 0;
                }
            }
            model.ResetParty();
            AmbitionApp.SendMessage(InventoryMessages.UNEQUIP, ItemType.Outfit); // Sends broadcast
            AmbitionApp.SendMessage(GameMessages.ADD_EXHAUSTION);
        }
    }
}
