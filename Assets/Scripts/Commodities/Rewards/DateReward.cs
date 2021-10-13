using System;
using System.Collections.Generic;
using System.Globalization;
namespace Ambition
{
    public class DateReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            int year = 0;
            int month = FindMonth(reward.ID);
            int day = reward.Value % 100;
            if (month == 0)
            {
                month = (int)(reward.Value * .01);
                year = (int)(month * .01);
                month = month % 100;
            }
            if (month > 0)
            {
                CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
                CalendarModel calendar = AmbitionApp.Calendar;
                int currDay = calendar.Day;
                RendezVO[] rendezs;
                DateTime date = new DateTime(year <= 0 ? 1789 : year, month, day);
                calendar.Day = day = date.Subtract(AmbitionApp.Calendar.StartDate).Days;
                foreach(KeyValuePair<string,CharacterVO> kvp in characters.Characters)
                {
                    if (kvp.Value.LiaisonDay >= 0 && kvp.Value.LiaisonDay < day)
                    {
                        rendezs = AmbitionApp.Calendar.GetOccasions<RendezVO>(kvp.Value.LiaisonDay);
                        characters.Characters[kvp.Key].LiaisonDay = -1;
                        Array.ForEach(rendezs, r => r.RSVP = RSVP.Declined);
                    }
                }
                AmbitionApp.Story.Update(true);
                AmbitionApp.UFlow.Reset();
                AmbitionApp.UFlow.Invoke(FlowConsts.DAY_FLOW_CONTROLLER);
            }
        }

        private static int FindMonth(string monthName)
        {
            if (string.IsNullOrEmpty(monthName)) return 0;
            string[] names = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames;
            monthName = monthName.ToLower();
            for (int i = names.Length - 1; i >= 0; --i)
            {
                if (names[i].ToLower().Equals(monthName)) return i + 1;
            }
            return 0;
        }
    }
}
