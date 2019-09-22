using System;
using Core;
using Util;
namespace Ambition
{
    public class GenerateOutfitCmd : ICommand<ItemVO>
    {
        public void Execute(ItemVO item)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            string style = RNG.TakeRandom(AmbitionApp.GetPhrases("outfit.style"));
            int modesty = GenerateRandom();
            int luxury = GenerateRandom();

            OutfitWrapperVO.SetState(item, ItemConsts.NOVELTY, 100);
            OutfitWrapperVO.SetState(item, ItemConsts.MODESTY, modesty);
            OutfitWrapperVO.SetState(item, ItemConsts.LUXURY, luxury);
            OutfitWrapperVO.SetState(item, ItemConsts.STYLE, style);

            item.Price = Math.Abs(modesty) + Math.Abs(luxury);
            if (style != inventory.Style)
                item.Price = (int)(item.Price * inventory.OutOfStyleMultiplier);

            item.Type = ItemType.Outfit;
            item.Name = AmbitionApp.GetString("outfit", new System.Collections.Generic.Dictionary<string, string>()
            {
                {"%s",style},
                {"%d",RNG.TakeRandom(AmbitionApp.GetPhrases("outfit.dress"))}
            });
        }

        private int GenerateRandom() => (int)(100 * Math.Sin(.01f * RNG.Generate(101)));
    }
}
