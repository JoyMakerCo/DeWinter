using System;
namespace Ambition
{
    public struct RewardRange
    {
        public CommodityType Type;
        public string ID;
        public int Low;
        public int High;

        public RewardRange(CommodityType type, string id, int low, int high)
        {
            Type = type;
            ID = id;
            Low = low;
            High = high;
        }

        public RewardRange(ref RewardRange arg)
        {
            Type = arg.Type;
            ID = arg.ID;
            Low = arg.Low;
            High = arg.High;
        }

        public RewardRange (ref CommodityVO arg)
        {
            Type = arg.Type;
            ID = arg.ID;
            Low = arg.Value;
            High = arg.Value;
        }
    }
}
