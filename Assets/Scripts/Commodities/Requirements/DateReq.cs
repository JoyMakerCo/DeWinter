using System;
using System.Globalization;

namespace Ambition
{
    public static class DateReq
    {
        // ID is the query month and Value is the Query date
        public static bool Check(RequirementVO req)
        {
            int year = 0;
            int month = FindMonth(req.ID);
            int day = req.Value % 100;
            if (month == 0)
            {
                month = (int)(req.Value * .01);
                year = (int)(month * .01);
                month = month % 100;
            }
            if (month == 0) return false;
            if (year == 0) year = 1789;
            DateTime date = new DateTime(year, month, day);
            RequirementVO requirement = new RequirementVO()
            {
                Value = date.Subtract(AmbitionApp.Calendar.StartDate).Days,
                Operator = req.Operator
            };
            return RequirementsSvc.Check(requirement, AmbitionApp.Calendar.Day);
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
