using System;
using Core;

namespace DeWinter
{
	public class CheckUprisingDayCmd : ICommand<CalendarDayVO>
	{
		public void Execute (CalendarDayVO day)
		{
			CalendarModel imod = DeWinterApp.GetModel<CalendarModel>();
			if (day.Day == imod.uprisingDay)
			{
				string victoriousPower;

		        //Establish each Faction's final Power
		        float crownFinalPower = GameData.factionList["Crown"].Power * 100;
		        float revolutionFinalPower = GameData.factionList["Third Estate"].Power * 100;
		        if(GameData.factionList["Church"].Allegiance > 0)
		        {
		            crownFinalPower += (Math.Abs((float)(GameData.factionList["Church"].Allegiance / 2)) * GameData.factionList["Church"].Power);
		        } else if (GameData.factionList["Church"].Allegiance < 0)
		        {
		            revolutionFinalPower += (Math.Abs((float)(GameData.factionList["Church"].Allegiance / 2)) * GameData.factionList["Church"].Power);
		        }
		        if (GameData.factionList["Military"].Allegiance > 0)
		        {
		            crownFinalPower += (Math.Abs((float)(GameData.factionList["Military"].Allegiance / 2)) * GameData.factionList["Military"].Power);
		        } else if (GameData.factionList["Military"].Allegiance < 0)
		        {
		            revolutionFinalPower += (Math.Abs((float)(GameData.factionList["Military"].Allegiance / 2)) * GameData.factionList["Military"].Power);
		        }
		        if (GameData.factionList["Bourgeoisie"].Allegiance > 0)
		        {
		            crownFinalPower += (Math.Abs((float)(GameData.factionList["Bourgeoisie"].Allegiance / 2)) * GameData.factionList["Bourgeoisie"].Power);
		        }
		        else if (GameData.factionList["Bourgeoisie"].Allegiance < 0)
		        {
		            revolutionFinalPower += (Math.Abs((float)(GameData.factionList["Bourgeoisie"].Allegiance / 2)) * GameData.factionList["Bourgeoisie"].Power);
		        }


		        //Compare Final Powers (who won and by what degree?)
				victoriousPower = crownFinalPower >= revolutionFinalPower ? "Crown" : "Third Estate";
		//		isDecisive = Math.Abs(crownFinalPower - revolutionFinalPower) > 50;
				
		        //Calculate Player Allegiance
		        if(GameData.factionList["Crown"].playerReputation > GameData.factionList["Third Estate"].playerReputation)
		        {
					GameData.Allegiance = "Crown";
		        } else if (GameData.factionList["Third Estate"].playerReputation > GameData.factionList["Crown"].playerReputation)
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
		        DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE,"Game_EndScreen");
	       	}
    	}		
	}
}