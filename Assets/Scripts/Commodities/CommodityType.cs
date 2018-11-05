using System;

namespace Ambition
{
	public enum CommodityType
	{
		Livre,		// Amount
		Reputation,	// Amount
		Gossip,		// Gossip localization key, 1 or -1
		Item,		// Item name, Quantity
		Enemy,		// Notable, 1 or -1
		Devotion,	// Notable, amount
		Servant,	// Servant id, 1 or -1
		Message,	// Message id, amount (may not matter)
        Incident,   // Incident id
        Location,   // Location Pin Name
        Party,      // Party ID; RSVP value

        // These are exclusively for checking Requirements/Objectives
        Date,       // Amount = ticks value of Date (Requirement Only)
        Mark        // In party objctives, when the Mark's room is cleared (value and ID ignored)
	}
}
