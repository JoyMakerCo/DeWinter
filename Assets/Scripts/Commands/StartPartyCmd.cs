﻿using System;
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

	        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
	        // TODO: Passive buff system
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			ItemVO accessory;
			if (inventory.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory))
			{
				switch(accessory.Name)
				{
					case "Garter Flask":
						model.AddBuff(GameConsts.DRINK, ItemConsts.ACCESSORY, 1.0f, 1.0f);
						break;
					case "Fascinator":
						model.ItemEffect = true;
						break;
				}
			}

			model.Drink = 0;
			model.Intoxication = 0;

			//Damage the Outfit's Novelty, how that the Confidence has already been Tallied
			model.TurnsLeft = model.Party.Turns;
			AmbitionApp.SendMessage<OutfitVO>(InventoryMessages.DEGRADE_OUTFIT, AmbitionApp.GetModel<GameModel>().Outfit);
			if (!string.IsNullOrEmpty(model.Party.IntroText))
			{
				AmbitionApp.OpenMessageDialog(model.Party.IntroText);
			}
		}
	}
}
