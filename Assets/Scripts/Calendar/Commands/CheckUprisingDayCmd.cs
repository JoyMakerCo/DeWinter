using System;
using Core;

namespace Ambition
{
	public class CheckUprisingDayCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			FactionModel fmod = AmbitionApp.GetModel<FactionModel>();

			FactionType winner;

	        //Establish each Faction's final Power
	        float crownFinalPower = fmod[FactionType.Crown].Power * 100;
	        float revolutionFinalPower = fmod[FactionType.Revolution].Power * 100;
	        if(fmod[FactionType.Church].Allegiance > 0)
	        {
	            crownFinalPower += (Math.Abs((float)(fmod[FactionType.Church].Allegiance / 2)) * fmod[FactionType.Church].Power);
	        } else if (fmod[FactionType.Church].Allegiance < 0)
	        {
	            revolutionFinalPower += (Math.Abs((float)(fmod[FactionType.Church].Allegiance / 2)) * fmod[FactionType.Church].Power);
	        }
	        if (fmod[FactionType.Military].Allegiance > 0)
	        {
	            crownFinalPower += (Math.Abs((float)(fmod[FactionType.Military].Allegiance / 2)) * fmod[FactionType.Military].Power);
	        } else if (fmod[FactionType.Military].Allegiance < 0)
	        {
	            revolutionFinalPower += (Math.Abs((float)(fmod[FactionType.Military].Allegiance / 2)) * fmod[FactionType.Military].Power);
	        }
	        if (fmod[FactionType.Bourgeoisie].Allegiance > 0)
	        {
	            crownFinalPower += (Math.Abs((float)(fmod[FactionType.Bourgeoisie].Allegiance / 2)) * fmod[FactionType.Bourgeoisie].Power);
	        }
	        else if (fmod[FactionType.Bourgeoisie].Allegiance < 0)
	        {
	            revolutionFinalPower += (Math.Abs((float)(fmod[FactionType.Bourgeoisie].Allegiance / 2)) * fmod[FactionType.Bourgeoisie].Power);
	        }


	        //Compare Final Powers (who won and by what degree?)
			winner = crownFinalPower >= revolutionFinalPower ? FactionType.Crown : FactionType.Revolution;
            //		isDecisive = Math.Abs(crownFinalPower - revolutionFinalPower) > 50;

            GameModel model = AmbitionApp.GetModel<GameModel>();

	        //Calculate Player Allegiance
	        if(fmod[FactionType.Crown].Reputation > fmod[FactionType.Revolution].Reputation)
	        {
				model.Allegiance = FactionType.Crown;
	        } else if (fmod[FactionType.Revolution].Reputation > fmod[FactionType.Crown].Reputation)
	        {
	            model.Allegiance = FactionType.Revolution;
	        } else // If it's equal then you get shuffled onto the losing team of History
	        {
				model.Allegiance = FactionType.Neutral;
	        }

	        //Go to the End Screen
            // TODO? I guess?
			//if(model.Allegiance == winner)
	   //     {
				//GameData.playerVictoryStatus = "Political Victory";
	        //} else
	        //{
	        //    GameData.playerVictoryStatus = "Political Loss";
	        //}
	        AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE,"Game_EndScreen");
    	}		
	}
}
