using System;
using System.Globalization;

namespace Ambition
{
    public static class DateReq
    {
        // ID is the query month and Value is the Query date
        // Returns true iff today's date that the query match
        public static bool Check(RequirementVO req)
        {
            int month = Array.IndexOf(
                CultureInfo.CurrentCulture.DateTimeFormat.MonthNames,
                req.ID.ToLower(CultureInfo.CurrentCulture)) + 1;
            DateTime date = new DateTime(month > 7 ? 1788 : 1789, month, req.Value);
            int target = (int)((date.Ticks - AmbitionApp.GetModel<GameModel>().Date.Ticks)*.00001f);
            RequirementVO r = new RequirementVO()
            {
                Operator = req.Operator,
                Value = 0
            };
            return RequirementsSvc.Check(r, target); // TODO: Bound by dates in play
        }
    }
}
