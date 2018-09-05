using System;
using Newtonsoft.Json;

// TODO: Make this RewardVO
namespace Ambition
{
	[Serializable]
    public struct CommodityVO
	{
		public CommodityType Type;
		public string ID;
		public int Amount;

		public CommodityVO (CommodityType type, string id=null, int amount=0)
		{
			Type = type;
			ID = id;
			Amount = amount;
		}

		public CommodityVO (CommodityType type, int amount=0)
		{
			Type = type;
			Amount = amount;
            ID = null;
		}
	}
}
