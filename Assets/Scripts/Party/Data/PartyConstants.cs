using System;

namespace Ambition
{
	public static class PartyConstants
	{
		public const string PUNCHBOWL = "Punchbowl";
		public const string HOST = "Host";
        public const string DISLIKE = "dislike";
        public const string LIKE = "like";
        public const string NEUTRAL = "neutral";
    }

    public enum GuestState { PutOff, Bored, Interested, Charmed, Offended };

    [Serializable]
    public enum RSVP { Declined=-1, New, Accepted, Required };
}
