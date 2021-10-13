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

        public const string PARTY_LOC = "party.";
        public const string PARTY_NAME = PARTY_LOC + "name.";
        public const string PARTY_NAME_DEFAULT = PARTY_NAME + "default";
        public const string PARTY_DESCRIPTION = PARTY_LOC + "description.";
        public const string PARTY_INVITATION = PARTY_LOC + "invitation.";
        public const string PARTY_HOST = PARTY_LOC + "host.";
        public const string PARTY_INVITATION_LOC = "party_invitation";
        public const string PARTY_SIZE = "party_importance.";
        public const string PARTY_REASON = "party_reason.";
        public const string PARTY_FLUFF = "party_fluff";
        public const string PARTY_FLUFF_INTRO = "party_fluff_intro.";
        public const string PARTY_FLUFF_NOUN = "party_fluff_noun.";
        public const string PARTY_FLUFF_ADJECTIVE = "party_fluff_adjective.";
        public const string PARTY_IMPORTANCE = "party_importance.";
    }

    public enum GuestState { PutOff, Bored, Interested, Charmed, Offended };

    [Serializable]
    public enum RSVP { Declined=-1, New, Accepted, Required };
}
