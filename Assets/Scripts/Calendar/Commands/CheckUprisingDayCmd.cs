using System;
using Core;

namespace Ambition
{
	public class CheckUprisingDayCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			FactionModel fmod = AmbitionApp.GetModel<FactionModel>();

			string victoriousPower;

	        //Establish each Faction's final Power
	        float crownFinalPower = fmod["Crown"].Power * 100;
	        float revolutionFinalPower = fmod["Third Estate"].Power * 100;
	        if(fmod["Church"].Allegiance > 0)
	        {
	            crownFinalPower += (Math.Abs((float)(fmod["Church"].Allegiance / 2)) * fmod["Church"].Power);
	        } else if (fmod["Church"].Allegiance < 0)
	        {
	            revolutionFinalPower += (Math.Abs((float)(fmod["Church"].Allegiance / 2)) * fmod["Church"].Power);
	        }
	        if (fmod["Military"].Allegiance > 0)
	        {
	            crownFinalPower += (Math.Abs((float)(fmod["Military"].Allegiance / 2)) * fmod["Military"].Power);
	        } else if (fmod["Military"].Allegiance < 0)
	        {
	            revolutionFinalPower += (Math.Abs((float)(fmod["Military"].Allegiance / 2)) * fmod["Military"].Power);
	        }
	        if (fmod["Bourgeoisie"].Allegiance > 0)
	        {
	            crownFinalPower += (Math.Abs((float)(fmod["Bourgeoisie"].Allegiance / 2)) * fmod["Bourgeoisie"].Power);
	        }
	        else if (fmod["Bourgeoisie"].Allegiance < 0)
	        {
	            revolutionFinalPower += (Math.Abs((float)(fmod["Bourgeoisie"].Allegiance / 2)) * fmod["Bourgeoisie"].Power);
	        }


	        //Compare Final Powers (who won and by what degree?)
			victoriousPower = crownFinalPower >= revolutionFinalPower ? "Crown" : "Third Estate";
	//		isDecisive = Math.Abs(crownFinalPower - revolutionFinalPower) > 50;
			
	        //Calculate Player Allegiance
	        if(fmod["Crown"].Reputation > fmod["Third Estate"].Reputation)
	        {
				GameData.Allegiance = "Crown";
	        } else if (fmod["Third Estate"].Reputation > fmod["Crown"].Reputation)
	        {
	            GameData.Allegiance = "Third Estate";
	        } else // If it's equal then you get shuffled onto the losing team of History
	        {
				GameData.Allegiance = "Unknown";
	        }

	        //Go to the End Screen

			if(GameData.Allegiance == victoriousPower)
	        {
				GameData.playerVictoryStatus = "Political Victory";
	        } else
	        {
	            GameData.playerVictoryStatus = "Political Loss";
	        }
	        AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE,"Game_EndScreen");
    	}		
	}
}
