using System;
using Core;

namespace Ambition
{
	public class DrinkForConfidenceCmd : ICommand
	{
		public void Execute()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			if (model.Drink > 0)
	        {
	        	AmbitionApp.AdjustValue(GameConsts.DRINK, -1);
				AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, 20);

				// TODO: Modifiers system
	            int drinkStrength = model.Party.drinkStrength;

	            //Is the Player decent friends with the Military? If so, make them more alcohol tolerant!
	            if(AmbitionApp.GetModel<FactionModel>().Factions["Military"].ReputationLevel >= 3)
	            {
	                drinkStrength -= 3;
	            }
	            //Is the Player using the Snuff Box Accessory? If so, then decrease the Intoxicating Effects of Booze!
	            if (GameData.partyAccessory != null)
	            {
	                if (GameData.partyAccessory.Type == "Snuff Box")
	                {
	                    drinkStrength -= 5;
	                }
	            }
				AmbitionApp.AdjustValue(GameConsts.INTOXICATION, drinkStrength);
        	}
        }
	}
}