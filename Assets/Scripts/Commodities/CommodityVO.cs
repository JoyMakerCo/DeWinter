using System;
using Newtonsoft.Json;

// TODO: Make this RewardVO
namespace Ambition
{
	[Serializable]
	public class CommodityVO
	{
		public CommodityType Type;
		public string ID;
		public int Amount=0;

		public CommodityVO() {}

		public CommodityVO (CommodityType type, string id, int amount)
		{
			Type = type;
			ID = id;
			Amount = amount;
		}

		public CommodityVO (CommodityType type, int amount)
		{
			Type = type;
			Amount = amount;
		}
	}
}
