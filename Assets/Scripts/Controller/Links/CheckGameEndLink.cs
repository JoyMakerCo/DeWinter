using System;
using UFlow;
namespace Ambition
{
    public class CheckGameEndLink : ULink
    {
        private static readonly DateTime END_DATE = new DateTime(1789, 3, 22);
        override public bool Validate() => false;
        //{
        //    return AmbitionApp.GetModel<CalendarModel>().Today == END_DATE;
        //}
    }
}
