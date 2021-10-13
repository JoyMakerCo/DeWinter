using System;
using System.Collections.Generic;
using Util;
using Core;

namespace Ambition
{
	public class UpdateMerchantCmd : ICommand
	{
		public void Execute()
		{
            InventoryModel inventory = AmbitionApp.Inventory;
            // Ensure that the same market values persist throughout the day
            if (inventory.Market == null)
            {
                OutfitVO outfit;
                List<OutfitVO> defs = new List<OutfitVO>();
                inventory.Market = new List<ItemVO>();

                foreach(ItemVO definition in inventory.Items.Values)
                {
                    if (definition.Type == ItemType.Outfit && definition.Market && !string.IsNullOrEmpty(definition.AssetID))
                        defs.Add(inventory.Instantiate(definition.ID) as OutfitVO);
                }

                for (int i= inventory.NumMarketSlots-1; i>=0; --i)
                {
                    outfit = GenerateOutfit(inventory, defs);
                    inventory.Market.Add(outfit);
                }
            }
        }

        OutfitVO GenerateOutfit(InventoryModel inventory, List<OutfitVO> defs)
        {
            OutfitVO outfit = new OutfitVO()
            {
                Created = AmbitionApp.Calendar.Today,
                Modesty = 100 - RNG.Generate(201),
                Luxury = 100 - RNG.Generate(201)
            };
            outfit.Price = Math.Abs(outfit.Modesty) + Math.Abs(outfit.Luxury);
            if (outfit.Price < 10) outfit.Price = 10;

            int modesty = outfit.Modesty <= inventory.RisqueLimit
                ? -1
                : outfit.Modesty >= inventory.ModestLimit
                ? 1 : 0;

            int luxury = outfit.Luxury <= inventory.HumbleLimit
                ? -1
                : outfit.Modesty >= inventory.LuxuryLimit
                ? 1 : 0;

            List<OutfitVO> candidates = new List<OutfitVO>();
            foreach (OutfitVO def in defs)
            {
                if ((def.Modesty < 0) == (modesty < 0)
                    && (def.Modesty > 0) == (modesty > 0)
                    && (def.Luxury < 0) == (luxury < 0)
                    && (def.Luxury > 0) == (luxury > 0))
                {
                    candidates.Add(def);
                }
            }
            OutfitVO candidate = Util.RNG.TakeRandom(candidates);
            outfit.ID = candidate.ID;
            outfit.AssetID = candidate.AssetID;
            return outfit;
        }
    }
}
