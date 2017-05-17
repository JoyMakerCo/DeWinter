using System;
using Core;

namespace Ambition
{
	public class EndPartyCmd : ICommand
	{
	    public void Execute()
	    {
	    	PartyModel model = AmbitionApp.GetModel<PartyModel>();
	    	AmbitionApp.AdjustValue<int>(PartyConstants.TURNSLEFT, -model.TurnsLeft);

            //Distribute the Rewards into the Player's 'Accounts' in Game Data and the appropriate Inventories
			model.Rewards.ForEach(AmbitionApp.SendMessage<RewardVO>);
			OutfitInventory.PartyOutfit = null;
	        GameData.partyAccessory = null;
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_AfterPartyReport");
	    }
	}
}