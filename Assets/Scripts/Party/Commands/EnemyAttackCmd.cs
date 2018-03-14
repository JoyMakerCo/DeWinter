using System;
using Core;

namespace Ambition
{
	public class EnemyAttackCmd : ICommand<GuestVO>
	{
		public void Execute(GuestVO guest)
		{
			EnemyVO enemy = guest as EnemyVO;
			if (enemy == null) return;

			if (enemy.State != GuestState.Charmed) // No dice if the enemy is dazed
			{
		        //Check for Charmed Guests, this is necessary for the Attack Check below
				MapModel model = AmbitionApp.GetModel<MapModel>();
		        GuestVO[] guests = Array.FindAll(model.Room.Guests, g => g.State == GuestState.Charmed);
		        if (guests.Length == 0) return; // Early Out

				int attackNumber = UnityEngine.Random.Range(0, 5);
		        switch (attackNumber)
		        {
		            case 1:
		                //1 = Monopolize Conversation (Lose a Turn)
		                //TODO: Need player-facing messaging!
//		                AmbitionApp.SendMessage(PartyMessages.END_TURN);
		                break;
		            case 2:
		                //2 = Rumor Monger (Lower the Opinion of all uncharmed Guests)
		                guests = Array.FindAll(model.Room.Guests, g => g.State != GuestState.Charmed);
		                foreach (GuestVO g in guests)
		                {
		                	g.Opinion -= 10;
		                }
		                break;
		            case 3:
		                //3 = Belittle (Sap your Confidence)
						AmbitionApp.GetModel<PartyModel>().Confidence -= 20;
		                break;
		            case 4:
		                //4 = Antagonize (Uncharm a Charmed Guest, if there is one)
		                if (guests.Length > 1)
		                {
							guest = guests[UnityEngine.Random.Range(1, guests.Length-1)];
							if (guest != enemy) guest.Opinion = 90;
			                else guests[0].Opinion = 90;
			            }
		                break;
		        }
		        if (attackNumber != 0)
		        {
		        	AmbitionApp.SendMessage<EnemyVO>(PartyMessages.ENEMY_RESET, enemy);	
		        }
			}
		}
	}
}