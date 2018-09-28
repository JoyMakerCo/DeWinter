using System;
using Core;

namespace Ambition
{
	public class HostVictoryCheckCmd : ICommand
	{
		public void Execute()
		{
			RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
			if (room != null && room.HostHere)
			{
/*				PartyModel model = AmbitionApp.GetModel<PartyModel>();
				switch(model.Party.Host.State)
				{
					case GuestState.Charmed:
					// YOU WIN!
						break;

					case GuestState.PutOff:
					// YOU FUCKED UP!!
						break;
				}
*/			}
		}
	}
}