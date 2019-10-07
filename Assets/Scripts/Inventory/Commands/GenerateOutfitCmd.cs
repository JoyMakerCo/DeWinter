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
            int modesty = RNG.Generate(101);
            int luxury = RNG.Generate(101);

            OutfitWrapperVO.SetState(item, ItemConsts.NOVELTY, 100);
            OutfitWrapperVO.SetState(item, ItemConsts.MODESTY, modesty);
            OutfitWrapperVO.SetState(item, ItemConsts.LUXURY, luxury);

            item.Price = Math.Abs(modesty) + Math.Abs(luxury);

            item.Type = ItemType.Outfit;

            var luxuryAdjective = Map(luxury, AmbitionApp.GetPhrases("outfit.luxury"));
            var modestyAdjective = Map(modesty, AmbitionApp.GetPhrases("outfit.modesty"));
            
            var adjective = luxuryAdjective.Length < modestyAdjective.Length ? luxuryAdjective : modestyAdjective;

            // room for both?
            if ((modestyAdjective.Length + luxuryAdjective.Length < 17) && (luxuryAdjective != modestyAdjective))
            {
                adjective = modestyAdjective + " " + luxuryAdjective;
            }

            // -- option: adjectives without style code
            item.Name = AmbitionApp.GetString("outfit", new System.Collections.Generic.Dictionary<string, string>()
            {
                {"%s",adjective},
                {"%d",RNG.TakeRandom(AmbitionApp.GetPhrases("outfit.dress"))}
            });

            // -- option: adjectives with style code
            //item.Name = AmbitionApp.GetString("outfit", new System.Collections.Generic.Dictionary<string, string>()
            //{
            //    {"%s",adjective + " " + style},
            //    {"%d",RNG.TakeRandom(AmbitionApp.GetPhrases("outfit.dress"))}
            //});
        }

        private string Map(int stat, string[] phrases) => phrases[(int)(phrases.Length * (.5f + stat * .00499f))];

    }
}
