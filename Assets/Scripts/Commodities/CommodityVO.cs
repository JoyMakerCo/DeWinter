using System;
using Newtonsoft.Json;

// TODO: Make this RewardVO
namespace Ambition
{
	[Serializable]
    public class CommodityVO
	{
        [JsonProperty("type")]
        public CommodityType Type;

        [JsonProperty("id")]
        public string ID;

        [JsonProperty("value")]
        public int Value;

        public CommodityVO() { }
        public CommodityVO(CommodityVO commodity) : this(commodity.Type, commodity.ID, commodity.Value) { }

        public CommodityVO(CommodityType type)
        {
            Type = type;
        }

        public CommodityVO(CommodityType type, int amount = 0)
        {
            Type = type;
            Value = amount;
            ID = null;
        }

        public CommodityVO (CommodityType type, string id=null, int value=0)
		{
			Type = type;
			ID = id;
			Value = value;
		}
	}
}
