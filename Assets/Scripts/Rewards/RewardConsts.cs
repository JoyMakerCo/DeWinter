using System;

namespace Ambition
{
	public enum RewardType
	{
		Livre,		// Amount
		Confidence,	// Amount
		Reputation,	// Amount
		Gossip,		// Gossip localization key, 1 or -1
		Item,		// Item name, Quantity
		Enemy,		// Notable, 1 or -1
		Devotion,	// Notable, amount
		Faction,	// Faction name, amount
		Servant,	// Servant id, 1 or -1
		Message		// Message id, amount (may not matter)
	}

	public class RewardConsts
	{
		public const string VALUE = "ValueReward";
		public const string GOSSIP = "GossipReward";
		public const string ITEM = "ItemReward";
		public const string ENEMY = "EnemyReward";
		public const string DEVOTION = "DevotionReward";
		public const string FACTION = "FactionReward";
		public const string SERVANT = "ServantReward";
		public const string MESSAGE = "InternalMessageReward";
	}
}