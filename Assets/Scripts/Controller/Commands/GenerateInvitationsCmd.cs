using System;
using Util;
namespace Ambition
{
    public class GenerateInvitationsCmd : Core.ICommand<CalendarModel>
    {
        public void Execute(CalendarModel calendar)
        {
            ChapterVO chapter = AmbitionApp.Game.GetChapter();
            if (chapter.TrivialPartyChance + chapter.DecentPartyChance + chapter.GrandPartyChance > 0
                && AmbitionApp.CheckRequirements(AmbitionApp.Game.StartInvitationsReqirements)
                && RNG.Generate(0, 3) == 0)
            {
                int day = calendar.Day + RNG.Generate(1, 8) + RNG.Generate(1, 8); // +2d8 days
                if (AmbitionApp.GetEvent(day) == null)
                {
                    PartyVO party = new PartyVO() { Day = day };
                    AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
                }
            }
        }
    }
}
