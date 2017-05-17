using System;
using UnityEngine;
using Core;
using System.Collections.Generic;

namespace Ambition
{
	public class StartPartyCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			if (model.Party == null)
			{
				Debug.Log("No Party! :(");
			}
	        else if (OutfitInventory.PartyOutfit != null)
	        {
	        	model.Rewards = new List<RewardVO>();
	            AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTY);
	        } else
	        {
	            Debug.Log("No Outfit selected :(");
	        }
		}
	}
}