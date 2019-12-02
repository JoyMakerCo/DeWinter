using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
    // Ends the current party and nullifies the reference in the model.
	public class EndPartyCmd : ICommand
	{
	    public void Execute()
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
            game.Exhaustion.Value = game.Exhaustion.Value+1;

            AmbitionApp.UnregisterModel<PartyModel>();
        }
    }
}
