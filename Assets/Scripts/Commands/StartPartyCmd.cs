﻿using System;
using UnityEngine;
using Core;
using System.Collections.Generic;

namespace Ambition
{
	// todo: make a party state
	public class StartPartyCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();

	        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
	        // TODO: Passive buff system
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			MapModel map = AmbitionApp.GetModel<MapModel>();

			ItemVO accessory;
			if (inventory.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory != null)
			{
				switch(accessory.Name)
				{
					case "Garter Flask":
						model.AddBuff(GameConsts.DRINK, ItemConsts.ACCESSORY, 1.0f, 1.0f);
						break;
					case "Fascinator":
                        AmbitionApp.GetModel<ConversationModel>().ItemEffect = true;
						break;
				}
			}

			model.Drink = 0;
			model.Intoxication = 0;
// TODO: Properly calculate
model.Confidence = model.StartConfidence = model.MaxConfidence = 120;

			//Damage the Outfit's Novelty, now that the Confidence has already been Tallied
			AmbitionApp.SendMessage<OutfitVO>(InventoryMessages.DEGRADE_OUTFIT, AmbitionApp.GetModel<GameModel>().Outfit);
			string introText = AmbitionApp.GetString("party.intro." + model.Party.ID + ".body");
			if (introText != null) AmbitionApp.OpenMessageDialog("party.intro." + model.Party.ID);

			map.Room = map.Map.Entrance;
		}
	}
}
