using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class OutfitVO : ItemVO
    {
        public OutfitVO() : base()
        {
            Type = ItemType.Outfit;
            TimesWorn = 0;
            Novelty = 100;
            Equipped = false;
        }

        public OutfitVO(ItemVO item) : base(item, item.State)
        {
            Type = ItemType.Outfit;
            if (GetStat(ItemConsts.TIMES_WORN) == null)
                TimesWorn = 0;
            if (GetStat(ItemConsts.NOVELTY) == null)
                Novelty = 100;
        }

        public int Novelty
        {
            get => GetIntStat(ItemConsts.NOVELTY);
            set => SetState(ItemConsts.NOVELTY, value);
        }

        public int Luxury
        {
            get => GetIntStat(ItemConsts.LUXURY);
            set => SetState(ItemConsts.LUXURY, value);
        }

        public int Modesty
        {
            get => GetIntStat(ItemConsts.MODESTY);
            set => SetState(ItemConsts.MODESTY, value);
        }

        public int TimesWorn
        {
            get => GetIntStat(ItemConsts.TIMES_WORN);
            set => SetState(ItemConsts.TIMES_WORN, value);
        }

        public string GetStat(string stat) => stat == null || State == null
            ? null
            : State.TryGetValue(stat, out string result)
            ? result
            : null;

        public int GetIntStat(string stat) => int.TryParse(GetStat(stat), out int result) ? result : 0;

        public void SetState(string stat, int value) => SetState(stat, value.ToString());
        public void SetState(string stat, string value)
        {
            if (State == null) State = new Dictionary<string, string>();
            State[stat] = value;
        }
    }
}
