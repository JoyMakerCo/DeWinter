using System;
using UFlow;
namespace Ambition
{
    public class CheckChapterLink : ULink
    {
        public override bool Validate() => AmbitionApp.GetModel<CalendarModel>().GetEvent<ChapterVO>() != null;
    }
}
