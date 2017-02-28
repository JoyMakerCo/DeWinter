using System;
using Core;

namespace DeWinter
{
	public class GoToRoomCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			// If Current Room is null, you're probably jumping into the foyer.
			MapModel model = DeWinterApp.GetModel<MapModel>();
			PartyModel partyModel = DeWinterApp.GetModel<PartyModel>();
			Random rnd = new Random();

			// Make sure the destination is connected to the current room
			if (model.Room == null || model.Room.IsNeighbor(room))
			{
				// Make sure the player can move to the next room
				if (model.Room == null ||  rnd.Next(100) < model.Room.MoveThroughChance)
				{
					model.Room = room;
					// Doing this will broadcast a message.

					// Reveal neighboring rooms
					for(int i=room.Neighbors.Length-1; i>=0; i--)
					{
						room.Neighbors[i].Revealed = true;
						DeWinterApp.SendMessage<RoomVO>(MapMessage.ROOM_REVEALED, room.Neighbors[i]);
					}

					// Drunk check
					if (Array.IndexOf(room.Features, PartyConstants.PUNCHBOWL) >= 0)
		            {
						partyModel.DrinkAmount = partyModel.MaxDrinkAmount;
					}
					else if (!room.Cleared
						&& partyModel.DrinkAmount < partyModel.MaxDrinkAmount
		            	&& GameData.factionList[partyModel.Party.faction].ReputationLevel >= 5
		            	&& rnd.Next(0, 4) == 0)
			        {
						partyModel.DrinkAmount = partyModel.MaxDrinkAmount;
						DeWinterApp.SendMessage<Party>(PartyConstants.SHOW_DRINK_MODAL, partyModel.Party);
			        }
				}
				else
				{
					// Denied! Player can't move through the room yet.
				}
			}
			else
			{
				// Not an adjoined room. What the hell kind of warlock do you think you are.
			}
		}
	}
}