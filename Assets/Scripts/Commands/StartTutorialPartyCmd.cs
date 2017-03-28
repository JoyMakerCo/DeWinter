using System;
using UnityEngine;
using Core;

namespace DeWinter
{
	public class StartTutorialPartyCmd : ICommand
	{
		public void Execute ()
		{
			if (OutfitInventory.PartyOutfit != null)
	        {
	            DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTY);
	        } else
	        {
	            Debug.Log("No Outfit selected :(");
	        }
		}
	}
}