using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ambition
{
    public class OutfitConfig : ScriptableObject
    {
        public OutfitElement[] Outfits;

        [Range(0,100)]
        public int ModestyLimit;
        [Range(0, 100)]
        public int RacyLimit;
        [Range(0, 100)]
        public int HumbleLimit;
        [Range(0, 100)]
        public int LuxuriousLimit;

        public Sprite GetOutfit(ItemVO item)
        {
            if (item.Type != ItemType.Outfit) return null;
            int value = OutfitWrapperVO.GetLuxury(item);
            OutfitLuxury luxury = value <= HumbleLimit
                ? OutfitLuxury.Humble
                : value >= LuxuriousLimit
                ? OutfitLuxury.Luxurious
                : OutfitLuxury.Average;
            value = OutfitWrapperVO.GetModesty(item);
            OutfitModesty modesty = value <= RacyLimit
                ? OutfitModesty.Racy
                : value >= ModestyLimit
                ? OutfitModesty.Modest
                : OutfitModesty.Average;
            foreach (OutfitElement element in Outfits)
            {
                if (modesty == element.Modesty && luxury == element.Luxury)
                {
                    return element.Asset;
                }
            }
            return null;
        }

        [Serializable]
        public struct OutfitElement
        {
            public Sprite Asset;
            public OutfitLuxury Luxury;
            public OutfitModesty Modesty;
        }

        public enum OutfitModesty
        {
            Modest, Average, Racy
        }

        public enum OutfitLuxury
        {
            Humble, Average, Luxurious
        }
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/OutfitConfig")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<OutfitConfig>("Outfit Config");
#endif
    }
}
