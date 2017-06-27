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
			OutfitInventoryModel omod = AmbitionApp.GetModel<OutfitInventoryModel>();
	        //Is the Player in the wrong Style (but matching) and are they Level 8 or higher?
	        if (omod.PartyOutfit.style != imod.CurrentStyle
	        	&& gm.Level >= 8
	        	&& omod.PartyOutfit.style == GameData.partyAccessory.States[ItemConsts.STYLE] as string)
	        {
	            //25% Chance
	            if(new Random().Next(4) == 0)
	            {
	                //Send Out a Relevant Pop-Up
	                Dictionary<string, string> substitutions = new Dictionary<string, string>(){
						{"$OLDSTYLE",imod.CurrentStyle},
						{"$NEWSTYLE",omod.PartyOutfit.style}};
					AmbitionApp.OpenMessageDialog("set_trend_dialog", substitutions);
	            }
	        }
		}
	}
}