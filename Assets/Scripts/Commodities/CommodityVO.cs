using System;
using Newtonsoft.Json;

// TODO: Make this RewardVO
namespace Ambition
{
	[Serializable]
    public struct CommodityVO
	{
        [JsonProperty("type")]
        public CommodityType Type;

        [JsonProperty("id")]
        public string ID;

        [JsonProperty("value")]
        public int Value;

		public CommodityVO (CommodityType type, string id=null, int amount=0)
		{
			Type = type;
			ID = id;
			Value = amount;
		}

		public CommodityVO (CommodityType type, int amount=0)
		{
			Type = type;
			Value = amount;
            ID = null;
		}
	}
}
