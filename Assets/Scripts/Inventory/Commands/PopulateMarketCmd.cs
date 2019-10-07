using System;
using System.Collections.Generic;
using Core;
using Util;

namespace Ambition
{
    public class PopulateMarketCmd : ICommand
    {
        public void Execute()
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            if (inventory.Market != null) return;

            ItemVO outfit;
            List<ItemVO> outfits = new List<ItemVO>();
            for (int i=inventory.NumMarketSlots-1; i>=0; i--)
            {
                outfit = new ItemVO();

                AmbitionApp.SendMessage(InventoryMessages.GENERATE_OUTFIT, outfit);
                //OutfitWrapperVO.SetState(outfit, ItemConsts.NOVELTY, 100);
                //OutfitWrapperVO.SetState(outfit, ItemConsts.MODESTY, RNG.Generate(-100, 101));
                //OutfitWrapperVO.SetState(outfit, ItemConsts.LUXURY, RNG.Generate(-100, 101));

                //var style = RNG.TakeRandom(inventory.Styles);
                //OutfitWrapperVO.SetState(outfit, ItemConsts.STYLE, style);

                //outfit.Name = style + " outfit " + (i+1).ToString();
                //outfit.Price = 5;

                outfits.Add(outfit);
            }
        }
    }
}
