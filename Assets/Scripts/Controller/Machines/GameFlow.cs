using System;
using Core;
using UFlow;
namespace Ambition
{
    public class GameFlow : UFlowConfig
    {
        public override void Configure()
        {
            State("UpdateCalendar");
            State("Chapter");
            State("ChapterInput");
            State("MorningIncident");
            State("Party");
            State("NextDayState");
            State("DayFlow");

            Bind<UpdateCalendarState>("UpdateCalendar");
            Bind<LoadSceneInput, string>("Chapter", SceneConsts.CHAPTER_PLACARD);
            Bind<MessageInput, string>("ChapterInput", GameMessages.COMPLETE);
            Bind<UMachine>("MorningIncident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<UMachine>("Party", FlowConsts.PARTY_CONTROLLER);
            Bind<AdvanceDayState>("NextDayState");
            Bind<UMachine>("DayFlow", FlowConsts.DAY_FLOW_CONTROLLER);
        }
    }
}
