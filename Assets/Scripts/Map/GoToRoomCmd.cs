using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class GoToRoomCmd : ICommand<RoomVO>
	{
        public void Execute(RoomVO room)
        {
            // If Current Room is null, you're probably jumping into the foyer.
            MapModel model = AmbitionApp.GetModel<MapModel>();
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();

            if (model.Room != null && !model.Room.Cleared)
            {
                int chance = model.Room.MoveThroughChance;
                InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
                ItemVO accessory;
                // TODO: Implement Item states
                if (inventory.Equipped.TryGetValue("accessory", out accessory)
                    && accessory.Name == "Cane")
                {
                    chance = ((chance < 90) ? (chance + 10) : 100);
                }
                //TODO: MoveThrough vs Ambush
                //					if (rnd.Next(100) < chance)
            }

            if (!room.Visited)
            {
                AmbitionApp.SendMessage(room.Actions);
                room.Visited = true;
            }

            model.Room = room;

            foreach (RoomVO rm in model.Map.Rooms)
            {
                if (!rm.Revealed && rm.IsAdjacentTo(room))
                {
                    rm.Revealed = true;
                    AmbitionApp.SendMessage(rm);
                }
            }

            // Fill yer glass
            if (Array.IndexOf(room.Features, PartyConstants.PUNCHBOWL) >= 0)
            {
                AmbitionApp.SendMessage(PartyMessages.REFILL_DRINK);
            }

            // At a certain reputation level, the player's glass may be filled without a punchbowl
            else if (!room.Cleared
                && partyModel.Drink < partyModel.MaxDrinkAmount
                && AmbitionApp.GetModel<FactionModel>()[partyModel.Party.Faction].Level >= 5
                && Util.RNG.Generate(0, 4) == 0)
            {
                partyModel.Drink = partyModel.MaxDrinkAmount;
                Dictionary<string, string> subs = new Dictionary<string, string>(){
                    {"$HOSTNAME", partyModel.Party.Host}};
                AmbitionApp.OpenMessageDialog("refill_wine_dialog", subs);
            }
        }
	}
}
