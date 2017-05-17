using System;

namespace Ambition
{
	public static class PartyConstants
	{
		public const string PUNCHBOWL = "Punchbowl";
		public const string HOST = "Host";

		public const string TURNSLEFT = "TURNSLEFT";

		// TODO: Use the dialog system
		public const string SHOW_DRINK_MODAL = "ShowDrinkModalMsg";

		public const string START_DANCING = "StartDancingMsg";
	}

	public enum GuestState { PutOff, Ambivalent, Charmed };
}