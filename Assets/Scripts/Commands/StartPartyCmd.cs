using System;
using UnityEngine;
using Core;

namespace Ambition
{
	public class StartPartyCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = Ambition.GetModel<PartyModel>();
			if (model.Party == null)
			{
				Debug.Log("No Party! :(");
			}
	        else if (OutfitInventory.PartyOutfit != null)
	        {
	            Ambition.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTY);
	        } else
	        {
	            Debug.Log("No Outfit selected :(");
	        }
		}
	}
}