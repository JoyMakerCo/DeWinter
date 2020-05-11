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
            State("EndIncident");
            State("EndGameDecision");
            State("EndGame");
            State("NextDayState", false);
            State("Estate", false);
            State("PartyDecision");
            State("Paris");

            State("UpdateCalendar", false);
            State("ChapterDecision");
            State("Chapter");
            State("DayDecision", false);
            State("Day");

            Link("Day", "StartIncident");
            Link("RequiredPartyDecision", "Estate");
            Link("UpdateCalendar", "DayDecision");
            Link("NextDayState", "UpdateCalendar");
            Link("EndGameDecision", "NextDayState");
            Link("Paris", "EndIncident");
            Link("Paris", "Estate");

            Bind<SendMessageState, string>("InitGame", CalendarMessages.UPDATE_CALENDAR);
            Bind<SendMessageState, string>("UpdateCalendar", CalendarMessages.UPDATE_CALENDAR);
            Bind<UMachine, string>("StartIncident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<UMachine, string>("Estate", FlowConsts.ESTATE_CONTROLLER);
            Bind<UMachine, string>("Party", FlowConsts.PARTY_CONTROLLER);
            Bind<UMachine, string>("Paris", FlowConsts.PARIS_CONTROLLER);
            Bind<UMachine, string>("EndIncident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<SendMessageState, string>("NextDayState", CalendarMessages.NEXT_DAY);
            Bind<SendMessageState, string>("EndGame", GameMessages.END_GAME);


            BindLink<LoadSceneLink, string>("InitGame", "InitChapter", SceneConsts.CHAPTER_PLACARD);
            BindLink<MessageLink, string>("InitChapter", "StartIncident", GameMessages.COMPLETE);
            BindLink<CheckChapterLink>("UpdateCalendar", "ChapterDecision");
            BindLink<MessageLink, string>("Chapter", "DayDecision", GameMessages.COMPLETE);
            BindLink<MessageLink, string>("Day", "StartIncident", GameMessages.COMPLETE);
            BindLink<CheckRequiredPartyLink>("RequiredPartyDecision", "Party");
            BindLink<CheckLocationLink>("Paris", "EndIncident");
            BindLink<CheckPartyLink>("PartyDecision", "Party");
            BindLink<CheckGameEndLink>("EndGameDecision", "EndGame");
            BindLink<LoadSceneLink, string>("ChapterDecision", "Chapter", SceneConsts.CHAPTER_PLACARD);
            BindLink<LoadSceneLink, string>("DayDecision", "Day", SceneConsts.DAILY_PLACARD);
        }
    }
}
