using System;

namespace Ambition
{
	public static class PartyMessages
	{
		public const string SHOW_MAP = "SHOW_MAP";
		public const string SHOW_ROOM = "SHOW_ROOM";

        public const string GO_TO_PARTY = "GO_TO_PARTY";
        public const string START_PARTY = "START_PARTY";
        public const string PARTY_STARTED = "PARTY_STARTED";
        public const string START_TURN = "START_TURN";
        public const string TURN = "TURN";
        public const string TURNS_LEFT = "TURNS_LEFT";
        public const string END_TURN = "END_TURN";  // Turns for Party
        public const string ROUND = "ROUND";        // Rounds for Conversation
        public const string START_ROUND = "START_ROUND";
        public const string START_CONVERSATION = "START_CONVERSATION";
        public const string TIME_EXPIRED = "TIME_EXPIRED";  // Ran out of time in the round
        public const string LEAVE_PARTY = "LEAVE_PARTY"; // Player explicitly leaves the party
        public const string END_PARTY = "END_PARTY";
        public const string END_ROUND = "END_ROUND";
        public const string END_CONVERSATION = "END_CONVERSATION"; //This means the player won
        public const string FLEE_CONVERSATION = "FLEE_CONVERSATION"; //This means the player lost
        public const string START_DANCING = "StartDancingMsg";
		public const string AMBUSH = "AMBUSH";
        public const string SELECT_GUEST = "SELECT_GUEST";
        public const string GUEST_SELECTED = "GUEST_SELECTED";
        public const string GUEST_IGNORED = "GUEST_IGNORED";
        public const string TARGET_GUEST = "TARGET_GUEST";
		public const string GUEST_TARGETED = "GUEST_TARGETED";
        public const string GUEST_REACTION_POSITIVE = "GUEST_REACTION_POSITIVE";
        public const string GUEST_REACTION_NEUTRAL = "GUEST_REACTION_NEUTRAL";
        public const string GUEST_REACTION_NEGATIVE = "GUEST_REACTION_NEGATIVE";
        public const string GUEST_CHARMED = "GUEST_CHARMED";
        public const string GUEST_OFFENDED = "GUEST_OFFENDED";
        public const string GUEST_REACTION_BORED = "GUEST_REACTION_BORED";
        public const string ENEMY_RESET = "ENEMY_RESET";
		public const string REPARTEE_BONUS = "REPARTEE_BONUS";
		public const string DRAW_REMARK = "DRAW_REMARK";
        public const string FREE_REMARK = "FREE_REMARK";
		public const string FILL_REMARKS = "FILL_REMARKS"; // This uses free cards to fill hand
        public const string DECK_SIZE = "DECK";
        public const string RESHUFFLE_REMARKS = "RESHUFFLE_REMARKS";
        public const string BURN_REMARKS = "BURN_REMARKS";
        public const string DRAW_REMARKS = "DRAW_REMARKS";
        public const string DISCARD = "DISCARD";
        public const string DRINK = "DRINK";
        public const string REFILL_DRINK = "REFILL_DRINK";
		public const string TELEGRAPH = "TELEGRAPH";
		public const string GUEST_REMARK = "GUEST_REMARK";
		public const string HOST_REMARK = "HOST_REMARK";
        public const string OBJECTIVES_COMPLETE = "OBJECTIVES_COMPLETE";
        public const string START_TUTORIAL_PARTY = "START_TUTORIAL_PARTY";
        public const string END_TUTORIAL_PARTY = "END_TUTORIAL_PARTY";
    }
}
