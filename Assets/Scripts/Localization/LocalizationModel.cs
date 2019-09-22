using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ambition
{
    [Core.Saveable]
    public class LocalizationModel : Core.Model
    {
        [JsonProperty("subsitutions")]
        public Dictionary<string, string> Substitutions = new Dictionary<string, string>();

        public void SetPlayerName()
        {
            Substitutions[LocalizationConsts.PLAYER_NAME] = AmbitionApp.GetModel<GameModel>().PlayerName;
        }

        public string Speaker
        {
            get => Substitutions[LocalizationConsts.CHARACTER];
            set => Substitutions[LocalizationConsts.CHARACTER] = value;
        }

        public string SetDate(DateTime date)
        {
            Dictionary<string, string> subs = new Dictionary<string, string>();
            Substitutions[LocalizationConsts.MONTH] = AmbitionApp.GetPhrases("month")[date.Month - 1];
            Substitutions[LocalizationConsts.DAY] = date.Day.ToString();
            Substitutions[LocalizationConsts.YEAR] = date.Year.ToString();
            Substitutions[LocalizationConsts.WEEKDAY] = AmbitionApp.GetPhrases("weekday")[(int)(date.DayOfWeek)];
            Substitutions[LocalizationConsts.SHORT_DATE] = AmbitionApp.GetString("short_date", Substitutions);
            return Substitutions[LocalizationConsts.DATE] = AmbitionApp.GetString("date", Substitutions);
        }

        public string Date => Substitutions[LocalizationConsts.DATE];
        public string ShortDate => Substitutions[LocalizationConsts.SHORT_DATE];

        public string SetLocation(Pin location)
        {
            return Substitutions[LocalizationConsts.LOCATION] = location.Name;
        }
    }
}
