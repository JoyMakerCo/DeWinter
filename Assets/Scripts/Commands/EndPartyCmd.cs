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
			model.TurnsLeft -= model.TurnsLeft;

            //Distribute the Rewards into the Player's 'Accounts' in Game Data and the appropriate Inventories
			model.Party.Rewards.ForEach(AmbitionApp.SendMessage<CommodityVO>);
			gm.LastOutfit = gm.Outfit;
			gm.Outfit = null;
	        GameData.partyAccessory = null;
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_ESTATE);
// TODO: Format actual rewards
			AmbitionApp.OpenMessageDialog(DialogConsts.AFTER_PARTY);
	    }
	}
}