using System;

// TODO: Make this RewardVO
namespace Ambition
{
	[Serializable]
	public class RewardVO
	{
		public RewardType Type;
		public string ID;
		public int Amount=0;

		public RewardVO() {}

		public RewardVO (RewardType type, string id, int amount=1)
		{
			Type = type;
			ID = id;
			Amount = amount;
		}

		public RewardVO (RewardType type, int amount=1)
		{
			Type = type;
			Amount = amount;
		}
	}
}
