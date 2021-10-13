using System;
using System.Collections.Generic;
using UnityEngine;
namespace Ambition
{
    public class PoliticsList : MonoBehaviour
    {
        private const string LAST_UPDATED_LOC = "estate.journal.politics.days_ago";

        public UnityEngine.UI.Text Updated;
        public PoliticsListItem[] List;

        private void OnEnable()
        {
            FactionModel model = AmbitionApp.GetModel<FactionModel>();
            DateTime date = AmbitionApp.Calendar.StartDate.AddDays(model.LastUpdated);
            Dictionary<string,string> months = AmbitionApp.GetPhrases("month");
            Dictionary<string, string> subs = new Dictionary<string, string>();
            months.TryGetValue(CalendarConsts.MONTH_LOC + (date.Month - 1), out string monthName);
            subs["%DAY%"] = date.Day.ToString();
            subs["%MONTH%"] = monthName;
            subs["%YEAR%"] = date.Year.ToString();
            string dateText = AmbitionApp.Localize(CalendarConsts.DATE, subs);
            subs.Clear();
            subs["$N"] = (AmbitionApp.Calendar.Day - model.LastUpdated).ToString();
            subs["$DATE"] = dateText;
            Updated.text = AmbitionApp.Localize(LAST_UPDATED_LOC, subs);
            Array.ForEach(List, i => i.Data = model.Standings.Find(f=>f.Faction == i.Faction));
        }
    }
}
