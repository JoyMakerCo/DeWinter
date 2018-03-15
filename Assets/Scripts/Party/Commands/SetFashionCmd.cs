using System;
using System.Collections.Generic;
using Core; 

namespace Ambition
{
	public class SetFashionCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
			GameModel gm = AmbitionApp.GetModel<GameModel>();
			InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
	        //Is the Player in the wrong Style (but matching) and are they Level 8 or higher?
	        if (gm.Outfit.Style != imod.CurrentStyle
	        	&& gm.Level >= 8
	        	&& gm.Outfit.Style == GameData.partyAccessory.State[ItemConsts.STYLE] as string)
	        {
	            //25% Chance
	            if(Util.RNG.Generate(0,4) == 0)
	            {
	                //Send Out a Relevant Pop-Up
	                Dictionary<string, string> substitutions = new Dictionary<string, string>(){
						{"$OLDSTYLE",imod.CurrentStyle},
						{"$NEWSTYLE",gm.Outfit.Style}};
					AmbitionApp.OpenMessageDialog("set_trend_dialog", substitutions);
	            }
	        }
		}
	}
}