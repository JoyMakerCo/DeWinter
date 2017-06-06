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
			Random rnd = new Random();

			if (partyModel.TurnsLeft <= 0)
			{
				UnityEngine.Debug.Log("Out of turns. Go home!");
			}

			// Make sure the destination is connected to the current room
			else if (model.Room != null && !model.Room.IsNeighbor(room))
			{
				UnityEngine.Debug.Log("Not an adjoined room. What the hell kind of warlock do you think you are?");
			}

			// Make sure the player can move to the next room
			else if (model.Room == null || rnd.Next(100) < model.Room.MoveThroughChance)
			{
				// Reveal neighboring rooms
				foreach (RoomVO neighbor in room.Neighbors)
				{
					if (neighbor != null) neighbor.Revealed = true;
				}

				UnityEngine.Debug.Log("Going to " + room.Name);

				// Doing this will broadcast a message.
				model.Room = room;

				// Fill yer glass
				if (Array.IndexOf(room.Features, PartyConstants.PUNCHBOWL) >= 0)
	            {
					partyModel.DrinkAmount = partyModel.MaxDrinkAmount;
				}

				// 
				else if (!room.Cleared
					&& partyModel.DrinkAmount < partyModel.MaxDrinkAmount
	            	&& GameData.factionList[partyModel.Party.faction].ReputationLevel >= 5
	            	&& rnd.Next(0, 4) == 0)
		        {
					partyModel.DrinkAmount = partyModel.MaxDrinkAmount;
					Dictionary<string, string> subs = new Dictionary<string, string>(){
						{"$HOSTNAME", partyModel.Party.Host.Name}};
					AmbitionApp.OpenMessageDialog("refill_wine_dialog", subs);
		        }
			}

			else
			{
				// Denied! Player can't move through the room yet.
			}
		}
	}
}