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
			
            model.Turn = model.Turns;

            //Distribute the Rewards into the Player's 'Accounts' in Game Data and the appropriate Inventories
			model.Party.Rewards.ForEach(AmbitionApp.SendMessage<CommodityVO>);
			gm.LastOutfit = gm.Outfit;
			gm.Outfit = null;
	        GameData.partyAccessory = null;

			AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeout);
			AmbitionApp.SendMessage(GameMessages.FADE_OUT);
	    }

		private void HandleFadeout()
		{
			AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeout);
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.AFTER_PARTY_SCENE);
		}
	}
}
