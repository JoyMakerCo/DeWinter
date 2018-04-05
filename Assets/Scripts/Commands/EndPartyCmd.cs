using System;
using Core;

namespace Ambition
{
	public class EndPartyCmd : ICommand
	{
	    public void Execute()
	    {
	    	PartyModel model = AmbitionApp.GetModel<PartyModel>();
			GameModel gm = AmbitionApp.GetModel<GameModel>();
			
			AmbitionApp.OpenDialog<PartyVO>(AfterPartyDialog.DIALOG_ID, model.Party);

			model.TurnsLeft -= model.TurnsLeft;

            //Distribute the Rewards into the Player's 'Accounts' in Game Data and the appropriate Inventories
			model.Party.Rewards.ForEach(AmbitionApp.SendMessage<CommodityVO>);
			gm.LastOutfit = gm.Outfit;
			gm.Outfit = null;
	        GameData.partyAccessory = null;
	    }
	}
}
