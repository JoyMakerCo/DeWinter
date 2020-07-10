using System;
using Core;
using UFlow;
namespace Ambition
{
    public class RegisterDayFlowControllerCommand : UFlowConfig
    {
        public override void Initialize()
        {
            State("InitGame");
            State("InitChapter");
            State("StartIncident");
            State("RequiredPartyDecision");
            State("Party");
            State("Evening");
            State("EndIncident");
            State("EndGameDecision");
            State("EndGame");
            State("EndGameIncident");
            State("BackToMenu");
            State("NextDayState", false);
            State("UpdateCalendar");
            State("ChapterDecision");
            State("Chapter");
            State("DayTransition");
            State("Day");
            State("Estate", false);
            State("PartyDecision");
            State("Paris");

            Link("UpdateCalendar", "DayTransition");
            Link("Day", "StartIncident");
            Link("RequiredPartyDecision", "Estate");
            Link("PartyDecision", "Party");
            Link("EndGameDecision", "NextDayState");
            Link("Paris", "Evening");
            Link("Paris", "Estate");

            Bind<UpdateCalendarState>("InitGame");
            Bind<UpdateCalendarState>("UpdateCalendar");
            Bind<UMachine, string>("StartIncident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<UMachine, string>("Estate", FlowConsts.ESTATE_CONTROLLER);
            Bind<UMachine, string>("Party", FlowConsts.PARTY_CONTROLLER);
            Bind<UMachine, string>("Paris", FlowConsts.PARIS_CONTROLLER);
            Bind<SetActivityState, ActivityType>("Evening", ActivityType.Evening);
            Bind<UMachine, string>("EndIncident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<SendMessageState, string>("NextDayState", CalendarMessages.NEXT_DAY);
            Bind<EndGameState>("EndGame");
            Bind<UMachine, string>("EndGameIncident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<SendMessageState, string>("BackToMenu", GameMessages.EXIT_GAME);

            BindLink<LoadSceneLink, string>("InitGame", "InitChapter", SceneConsts.CHAPTER_PLACARD);
            BindLink<MessageLink, string>("InitChapter", "StartIncident", GameMessages.COMPLETE);
            BindLink<CheckChapterLink>("UpdateCalendar", "ChapterDecision");
            BindLink<MessageLink, string>("Chapter", "DayTransition", GameMessages.COMPLETE);
            BindLink<LoadSceneLink, string>("DayTransition", "Day", SceneConsts.DAILY_PLACARD);
            BindLink<MessageLink, string>("Day", "StartIncident", GameMessages.COMPLETE);
            BindLink<CheckRequiredPartyLink>("RequiredPartyDecision", "Party");
            BindLink<CheckLocationLink>("Paris", "Evening");
            BindLink<CheckPartyLink>("PartyDecision", "Party");
            BindLink<CheckGameEndLink>("EndGameDecision", "EndGame");
            BindLink<LoadSceneLink, string>("ChapterDecision", "Chapter", SceneConsts.CHAPTER_PLACARD);
        }
    }
}
