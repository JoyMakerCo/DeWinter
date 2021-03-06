﻿namespace Ambition
{
	public enum CommodityType
	{
		Livre,		// Amount
		Reputation,	// Amount
		Gossip,		// Gossip localization key, 1 or -1
		Item,		// Item name, Quantity
        Enemy,      // Character, + or - integer (0 to 100)
        Favor,      // Character, + or - integer (0 to 100)
		Servant,	// Servant id, 1 or -1
		Message,	// Message id, amount (may not matter)
        Incident,   // Incident id
        Location,   // Location Pin Name
        Party,      // Party ID; RSVP value -1 = Declined, 0 = New, 1 = Accepted
        Credibility,// Credibility Value (+ or -)
        Peril,      // Peril Value (+ or -)

        FactionAllegiance,
        FactionPower,

        // These are exclusively for checking Requirements/Objectives
        Date,       // Value = Date in military time: YYYYDDMM
        Mark,       // In party objctives, when the Mark's room is cleared (value and ID ignored)
        Chance,      // Value = Chance out of 100 of success
        Exhaustion,  // Value = Exhaustion level; ID = null
        ActiveQuest, // Value = 1 if quest is active
        Random,      // Value = number to test against

        OutfitReaction,      // Outfit credibility shift
    }
}
