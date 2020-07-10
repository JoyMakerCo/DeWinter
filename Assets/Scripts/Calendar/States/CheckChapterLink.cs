using System;
using UFlow;
namespace Ambition
{
    public class CheckChapterLink : ULink
    {
        public override bool Validate()
        {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            return Array.Exists(model.Chapters, c => c.Date == calendar.Today);
        }
    }
}
