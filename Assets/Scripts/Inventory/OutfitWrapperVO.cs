using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public static class OutfitWrapperVO
    {
        public static int GetNovelty(ItemVO item) => GetIntStat(item, ItemConsts.NOVELTY);
        public static int GetLuxury(ItemVO item) => GetIntStat(item, ItemConsts.LUXURY);
        public static int GetModesty(ItemVO item) => GetIntStat(item, ItemConsts.MODESTY);

        public static string GetNoveltyText(ItemVO item )
        {
            Dictionary<string, string> phrases = AmbitionApp.GetPhrases("outfit.novelty");
            int value = GetIntStat(item, ItemConsts.NOVELTY);
            int index = Mathf.Clamp( (int)(phrases.Count * value * .01f), 0, phrases.Count-1 );

            return phrases["outfit.novelty." + index];
        }

        public static string GetLuxuryText(ItemVO item )
        {
            Dictionary<string, string> phrases = AmbitionApp.GetPhrases("outfit.luxury");
            int value = GetIntStat(item, ItemConsts.LUXURY);
            int index = Mathf.Clamp( (int)(phrases.Count * (0.5f + (value * .005f))), 0, phrases.Count-1 );

            return phrases["outfit.luxury." + index];
        }

        public static string GetModestyText(ItemVO item )
        {
            Dictionary<string, string> phrases = AmbitionApp.GetPhrases("outfit.modesty");
            int value = GetIntStat(item, ItemConsts.MODESTY);
            int index = Mathf.Clamp( (int)(phrases.Count * (0.5f + (value * .005f))), 0, phrases.Count-1 );

            return phrases["outfit.modesty." + index];
        }
        public static string GetStat(ItemVO item, string stat)
        {
            string str = null;
            return (item?.State?.TryGetValue(stat, out str) ?? false) ? str : null;
        }

        public static int GetIntStat(ItemVO item, string stat)
        {
            stat = GetStat(item, stat);
            return stat == null ? 0 : int.Parse(stat);
        }

        public static bool SetState(ItemVO outfit, string stat, int value) => SetState(outfit, stat, value.ToString());
        public static bool SetState(ItemVO outfit, string stat, string value)
        {
            if (outfit?.Type != ItemType.Outfit) return false;
            outfit.State = outfit.State ?? new Dictionary<string, string>();
            outfit.State[stat] = value;
            return true;
        }

        public static string GetDescription(ItemVO outfit)
        {
            int modestyIndex = 1+(int)(5 * (.5f + GetModesty(outfit) * .00499f));
            int luxuryIndex = 1+(int)(5 * (.5f + GetLuxury(outfit) * .00499f));

            var locDescriptionKey = string.Format( "outfit.description.{0}.{1}", modestyIndex, luxuryIndex );
            return AmbitionApp.Localize( locDescriptionKey );
        }
    }
}
