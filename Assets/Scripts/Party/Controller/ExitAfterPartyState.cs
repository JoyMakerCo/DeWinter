using System;
namespace Ambition
{
    public class ExitAfterPartyState : UFlow.UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            OccasionVO[] occasions = calendar.GetOccasions(OccasionType.Party);
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();

            calendar.Complete(model.Party?.Name, OccasionType.Party);
            model.EndParty();

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
