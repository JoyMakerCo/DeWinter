using System;
using System.Linq;
using UnityEngine;
using Core;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
    public class InitPartyState : UState
	{
        public override void OnEnterState()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO party = model.Party;
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            FactionVO faction = AmbitionApp.GetModel<FactionModel>()[party.Faction];
            OutfitVO outfit = inventory.Equipped[ItemConsts.OUTFIT] as OutfitVO;

            // TODO: Passive buff system
            //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
            ItemVO item;
            if (inventory.Equipped.TryGetValue(ItemConsts.ACCESSORY, out item) && item != null)
            {
                switch (item.Name)
                {
                    case "Garter Flask":
                        model.AddBuff(GameConsts.DRINK, ItemConsts.ACCESSORY, 1.0f, 1.0f);
                        break;
                    case "Fascinator":
                        AmbitionApp.GetModel<ConversationModel>().ItemEffect = true;
                        break;
                    case "Snuff Box":
                        model.Party.MaxIntoxication += 5;
                        break;
                }
            }

            //Is the Player decent friends with the Military? If so, make them more alcohol tolerant!
            // TODO: why?
            if (AmbitionApp.GetModel<FactionModel>()[FactionConsts.MILITARY].Level >= 3)
            {
                model.Party.MaxIntoxication += 3;
            }

            model.Drink = 0;
            model.Intoxication = 0;

            float outfitScore = 400 - Math.Abs(faction.Modesty - outfit.Modesty) - Math.Abs(faction.Luxury - outfit.Luxury);
            ConversationModel conversation = AmbitionApp.GetModel<ConversationModel>();
            int num = model.DeckSize
                           + (int)(outfitScore * (float)(outfit.Novelty) * 0.001f)
                           + faction.DeckBonus
                           + AmbitionApp.GetModel<GameModel>().DeckBonus;
            int[] remarkids = Enumerable.Range(0, num).ToArray();
            int index;
            int tmp;
            string interest;
            int numTargets;
            int targetIndex = (int)(num * .5f); // Fifty-fifty one or two targets

            conversation.Deck = new Queue<RemarkVO>();
            conversation.Discard = new List<RemarkVO>();
            for (int i = num-1; i >= 0; i--)
            {
                index = Util.RNG.Generate(i);
                tmp = remarkids[i];
                remarkids[i] = remarkids[index];
                remarkids[index] = tmp;
                interest = model.Interests[remarkids[i] % model.Interests.Length];
                numTargets = remarkids[i] > targetIndex ? 2 : 1;
                conversation.Deck.Enqueue(new RemarkVO(numTargets, interest));
            }

            // TODO: Commandify and insert after entering map for first time
            //Damage the Outfit's Novelty, now that the Confidence has already been Tallied
            AmbitionApp.SendMessage(InventoryMessages.DEGRADE_OUTFIT, outfit);

//         string introText = AmbitionApp.GetString("party.intro." + party.ID + ".body");
//          if (introText != null) AmbitionApp.OpenMessageDialog("party.intro." + party.ID);
        }
    }
}
