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
            if (phrases.Count == 0) return "";
            int value = GetIntStat(item, ItemConsts.NOVELTY);
            return new List<string>(phrases.Values)[(int)(.01f*phrases.Count*value)];
        }

        public static string GetLuxuryText(ItemVO item ) => GetStatText(ItemConsts.LUXURY, item);
        public static string GetModestyText(ItemVO item) => GetStatText(ItemConsts.MODESTY, item);

        public static string GetStatText(string stat, ItemVO item)
        {
            Dictionary<string, string> phrases = AmbitionApp.GetPhrases("outfit." + stat);
            if (phrases.Count == 0) return "";
            int value = GetIntStat(item, stat);
            return new List<string>(phrases.Values)[(int)(.005f*phrases.Count*(value+100))];
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
