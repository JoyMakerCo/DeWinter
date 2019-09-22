using System;
using System.Collections.Generic;
namespace Ambition
{
    public static class OutfitWrapperVO
    {
        public static int GetNovelty(ItemVO item) => GetIntStat(item, ItemConsts.NOVELTY);
        public static int GetLuxury(ItemVO item) => GetIntStat(item, ItemConsts.LUXURY);
        public static int GetModesty(ItemVO item) => GetIntStat(item, ItemConsts.MODESTY);
        public static string GetStyle(ItemVO item) => GetStat(item, ItemConsts.STYLE);

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
    }
}
