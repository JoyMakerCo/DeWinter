using System;
namespace Ambition
{
    public static class LocalizationConsts
    {
        // GLOBAL LOCALIZATION KEYS
        public const string PLAYER_NAME = "%PLAYER%";   // The Player Character's name
        public const string SPEAKER = "%SPEAKER%";      // The Speaking Character in an Incident Moment
        public const string CHARACTER_1 = "%CHARACTER1%"; // The 1st Character in an Incident Moment
        public const string CHARACTER_2 = "%CHARACTER2%"; // The 2nd Character in an Incident Moment
        public const string CHARACTER_3 = "%CHARACTER3%"; // The 3rd Character in an Incident Moment
        public const string CHARACTER_4 = "%CHARACTER4%"; // The 4th Character in an Incident Moment
        public const string MONTH = "%MONTH%";          // The Current Month
        public const string DAY = "%DAY%";              // The Current Day
        public const string YEAR = "%YEAR%";            // The Current Year
        public const string DATE = "%DATE%";            // The Current Date
        public const string SHORT_DATE = "%SHORTDATE%"; // The Current Date without year
        public const string WEEKDAY = "%WEEKDAY%";      // The Current Weekday
        public const string LOCATION = "%LOCATION%";    // The current Paris Location or Party Title

        public const string QUESTGOSSIPFACTION = "%QUESTGOSSIPFACTION%";    // Faction of the current gossip quest
        public const string QUESTTIME = "%QUESTTIME%";                      // Time limit of the current gossip quest
        public const string QUESTREWARD = "%QUESTREWARD%";                  // Reward description of the current gossip quest

        public const string OUTFITNAME = "$OUTFITNAME";
        public const string OUTFITTITLE = "$OUTFITTITLE";
        public const string OUTFITMODESTY = "$OUTFITMODESTY";
        public const string OUTFITLUXURY = "$OUTFITLUXURY";

        public const string PARTYFACTION = "$PARTYFACTIONNAME";
        public const string PARTYFACTIONMODESTY = "$PARTYFACTIONMODESTYPREFERENCE";
        public const string PARTYFACTIONLUXURY = "$PARTYFACTIONLUXURYPREFERENCE";

        public const string EXIT_PARTY = "calendar.btn.party";
        public const string EXIT_PARIS = "calendar.btn.paris";
        public const string EXIT_RENDEZVOUS = "calendar.btn.rendezvous";

        public const string Day = "day";
        public const string Days = "days";
    }
}
