using System;
namespace Ambition
{
    public class ExitAfterPartyState : UFlow.UState
    {
        public override void OnEnterState()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            Array.ForEach(calendar.GetEvents<PartyVO>(), calendar.Complete);
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            //if (inventory.Equipped.TryGetValue(ItemType.Outfit, out List<ItemVO> outfits))
            //{
            //    model.LastOutfit = outfits.Count > 0 ? outfits[0] : null;
            //}
            //else model.LastOutfit = null;

            // Add exhaustion
            GameModel game = AmbitionApp.GetModel<GameModel>();
            game.Exhaustion.Value = game.Exhaustion.Value + 1;

            AmbitionApp.UnregisterModel<PartyModel>();
        }
    }
}
