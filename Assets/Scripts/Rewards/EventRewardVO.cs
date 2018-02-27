using System;

// TODO: Make this RewardVO
namespace Ambition
{
	public class EventRewardVO
	{
		public RewardType Type;
		public string ID;
		public int Amount=0;

		public EventRewardVO() {}

		public EventRewardVO (RewardType type, string id, int amount)
		{
			Type = type;
			ID = id;
			Amount = amount;
		}

		public EventRewardVO (RewardType type, int amount)
		{
			Type = type;
			Amount = amount;
		}
	}
}
