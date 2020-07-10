using System;
namespace Ambition
{
    public class ExitAfterPartyState : UFlow.UState
    {
        public override void OnEnterState()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            model.EndParty();

            //Damage the Outfit's Novelty, now that the Confidence has already been Tallied
            if (inventory.Equipped.TryGetValue(ItemType.Outfit, out ItemVO item))
            {
                int novelty = OutfitWrapperVO.GetNovelty(item);
                OutfitWrapperVO.SetState(item, ItemConsts.NOVELTY, novelty - 1);
            }
            AmbitionApp.SendMessage(InventoryMessages.UNEQUIP, ItemType.Outfit);

            // Add exhaustion
            GameModel game = AmbitionApp.GetModel<GameModel>();
            game.Exhaustion.Value = game.Exhaustion.Value++;

            AmbitionApp.UnregisterCommand<PartyRewardCmd, CommodityVO>();
            AmbitionApp.UnregisterCommand<PartyRewardsCmd, CommodityVO[]>();
        }
    }
}
