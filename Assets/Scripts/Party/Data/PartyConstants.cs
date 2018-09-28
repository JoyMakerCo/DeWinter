using System;

namespace Ambition
{
	public static class PartyConstants
	{
		public const string PUNCHBOWL = "Punchbowl";
		public const string HOST = "Host";
	}

	public enum GuestState { PutOff, Bored, Interested, Charmed };

    [Serializable]
    public enum RSVP { Declined=-1, New, Accepted };
}
