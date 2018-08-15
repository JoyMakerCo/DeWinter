using System;

namespace Ambition
{
	public enum CommodityType
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
		Message,	// Message id, amount (may not matter)
        Incident,   // Incident id
        Location    // Location Pin Name
	}
}
