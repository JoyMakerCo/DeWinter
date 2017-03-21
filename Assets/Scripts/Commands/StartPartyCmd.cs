using System;
using UnityEngine;
using Core;

namespace DeWinter
{
	public class StartPartyCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = DeWinterApp.GetModel<PartyModel>();
			if (model.Party == null)
			{
				Debug.Log("No Party! :(");
			}
	        else if (OutfitInventory.PartyOutfit != null)
	        {
	            DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTY);
	        } else
	        {
	            Debug.Log("No Outfit selected :(");
	        }
		}
	}
}