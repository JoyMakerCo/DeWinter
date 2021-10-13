namespace Ambition
{
	public enum CommodityType
	{
		Livre,		// Amount
		Achievement,// ID = achievement ID [Formerly Reputation]
		Gossip,		// Gossip localization key, 1 or -1
		Item,		// Item name, Quantity
        Enemy,      // Character, + or - integer (0 to 100)
        Favor,      // Character, + or - integer (0 to 100)
		Servant,	// Servant id, 0=introduced/fired; 1=hired; 2=permanent
		Message,	// Message id, amount (may not matter)
        Incident,   // Incident id; 0 for incomplete or 1 for complete
        Location,   // ID = Location ID; Value = 1 (Add); Value = -1 (Remove)
        Party,      // ID = Party Name; Value = Days from today OR -1 to ignore
        Credibility,// Credibility Value (+ or -)
        Peril,      // Peril Value (+ or -)
        Allegiance,
        Power,
        Date,       // ID = Month or null; Value = Day (Year is 1789) or Date (Format is YYYYMMDD)
        Mark,       // In party objctives, when the Mark's room is cleared (value and ID ignored)
        Chance,      // Value = Chance out of 100 of success
        Exhaustion,  // Value = Exhaustion level; ID = null
        Quest, // Value = 1 if quest is active
        Random,      // Value = number to test against
        OutfitReaction,      // Outfit credibility shift
        Liaison,     // ID = Romance Option's Name; Value (≤0 = Not Dateable; >0 = Dateable)
        Misc,
        Rendezvous,  // ID = Character or Location of current Rendezvous; Value > 0: Required Character or Location
        RendezvousFavor, // Value = Difference in favor
        RendezvousOutfit, // Value = Difference in favor due to outfit
        Tutorial,     // ID = ID of tutorial being enabled
        Character,     // ID = characterID
    }
}
