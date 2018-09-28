using System;
using UnityEngine;
using Core;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
    public class InitPartyState : UState
	{
        public override void OnEnterState ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            FactionVO faction = AmbitionApp.GetModel<FactionModel>()[model.Party.Faction];
            OutfitVO outfit = inventory.Equipped[ItemConsts.OUTFIT] as OutfitVO;

            // TODO: Passive buff system
            //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
            ItemVO item;
            if (inventory.Equipped.TryGetValue(ItemConsts.ACCESSORY, out item) && item != null)
			{
				switch(item.Name)
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

            int outfitScore = 400 - Math.Abs(faction.Modesty - outfit.Modesty) - Math.Abs(faction.Luxury - outfit.Luxury);
            model.Confidence = model.MaxConfidence = model.StartConfidence = model.BaseConfidence +
                (int)((float)(outfitScore >> 1) * (float)(outfit.Novelty) * 0.01f) +
                faction.ConfidenceBonus +
                AmbitionApp.GetModel<GameModel>().ConfidenceBonus;

            // TODO: Commandify and insert after entering map for first time
            //Damage the Outfit's Novelty, now that the Confidence has already been Tallied
            AmbitionApp.SendMessage(InventoryMessages.DEGRADE_OUTFIT, outfit);
            string introText = AmbitionApp.GetString("party.intro." + model.Party.ID + ".body");
			if (introText != null) AmbitionApp.OpenMessageDialog("party.intro." + model.Party.ID);

            AmbitionApp.SendMessage(MapMessage.GENERATE_MAP, model.Party); //TODO: COmmandify
        }
    }
}
