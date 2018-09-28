using System;
using Core;

namespace Ambition
{
	public class EndPartyCmd : ICommand
	{
	    public void Execute()
	    {
	    	PartyModel model = AmbitionApp.GetModel<PartyModel>();
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			
            model.Turn = model.Turns;

            //Distribute the Rewards into the Player's 'Accounts' in Game Data and the appropriate Inventories
			model.Party.Rewards.ForEach(AmbitionApp.SendMessage<CommodityVO>);
            inventory.Equipped[ItemConsts.LAST_OUTFIT] = inventory.Equipped[ItemConsts.OUTFIT];
            inventory.Equipped.Remove(ItemConsts.OUTFIT);
            inventory.Equipped.Remove(ItemConsts.ACCESSORY);

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
