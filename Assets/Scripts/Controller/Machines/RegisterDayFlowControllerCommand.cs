using System;
using Core;
using UFlow;
namespace Ambition
{
    public class RegisterDayFlowControllerCommand : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterState<SendMessageState, string>("DayFlowController", "InitCharacter", GameMessages.INIT_CHARACTER);
            AmbitionApp.RegisterState<SendMessageState, string>("DayFlowController", "InitUpdate", CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterState<SendMessageState, string>("DayFlowController", "UpdateCalendar", CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterState<LoadSceneState, string>("DayFlowController", "Chapter", SceneConsts.CHAPTER_PLACARD);
            AmbitionApp.RegisterState<LoadSceneState, string>("DayFlowController", "InitChapter", SceneConsts.CHAPTER_PLACARD);
            AmbitionApp.RegisterState<LoadSceneState, string>("DayFlowController", "DayPlacard", SceneConsts.DAILY_PLACARD);
            AmbitionApp.RegisterMachineState("DayFlowController", "StartIncident", "IncidentController");
            AmbitionApp.RegisterState("DayFlowController", "RequiredPartyDecision");
            AmbitionApp.RegisterMachineState("DayFlowController", "Estate", "EstateController");
            AmbitionApp.RegisterState("DayFlowController", "PartyDecision");
            AmbitionApp.RegisterMachineState("DayFlowController", "Party", "PartyController");
            AmbitionApp.RegisterMachineState("DayFlowController", "Paris", "ParisMapController");
            AmbitionApp.RegisterState("DayFlowController", "LocationDecision");
            AmbitionApp.RegisterState("DayFlowController", "EndIncidentDecision");
            AmbitionApp.RegisterState("DayFlowController", "RestDecision");
            AmbitionApp.RegisterState<SendMessageState, string>("DayFlowController", "RestState", ParisMessages.REST);
            AmbitionApp.RegisterMachineState("DayFlowController", "EndIncident", "IncidentController");
            AmbitionApp.RegisterState("DayFlowController", "EndGameDecision");
            AmbitionApp.RegisterState<SendMessageState, string>("DayFlowController", "NextDayState", CalendarMessages.NEXT_DAY);
            AmbitionApp.RegisterState<SendMessageState, string>("DayFlowController", "EndGameState", GameMessages.END_GAME);

            AmbitionApp.RegisterLink("DayFlowController", "InitCharacter", "InitUpdate");
            AmbitionApp.RegisterLink("DayFlowController", "InitUpdate", "InitChapter");
            AmbitionApp.RegisterLink<MessageLink, string>("DayFlowController", "InitChapter", "StartIncident", GameMessages.COMPLETE);
            AmbitionApp.RegisterLink<CheckChapterLink>("DayFlowController", "UpdateCalendar", "Chapter");
            AmbitionApp.RegisterLink("DayFlowController", "UpdateCalendar", "DayPlacard");
            AmbitionApp.RegisterLink<MessageLink, string>("DayFlowController", "Chapter", "DayPlacard", GameMessages.COMPLETE);
            AmbitionApp.RegisterLink<MessageLink, string>("DayFlowController", "DayPlacard", "StartIncident", GameMessages.COMPLETE);

            AmbitionApp.RegisterLink("DayFlowController", "StartIncident", "RequiredPartyDecision");
            AmbitionApp.RegisterLink<CheckRequiredPartyLink>("DayFlowController", "RequiredPartyDecision", "Party");
            AmbitionApp.RegisterLink("DayFlowController", "RequiredPartyDecision", "Estate");
            AmbitionApp.RegisterLink("DayFlowController", "Estate", "PartyDecision");
            AmbitionApp.RegisterLink<CheckPartyLink>("DayFlowController", "PartyDecision", "Party");
            AmbitionApp.RegisterLink("DayFlowController", "PartyDecision", "Paris");
            AmbitionApp.RegisterLink("DayFlowController", "Paris", "LocationDecision");
            AmbitionApp.RegisterLink<CheckLocationLink>("DayFlowController", "LocationDecision", "RestDecision");
            AmbitionApp.RegisterLink("DayFlowController", "LocationDecision", "Estate");
            AmbitionApp.RegisterLink<RestAtHomeLink>("DayFlowController", "RestDecision", "RestState");
            AmbitionApp.RegisterLink("DayFlowController", "RestDecision", "RestState");
            AmbitionApp.RegisterLink("DayFlowController", "RestState", "EndIncident");
            AmbitionApp.RegisterLink("DayFlowController", "Party", "EndIncident");
            AmbitionApp.RegisterLink("DayFlowController", "EndIncident", "EndGameDecision");
            AmbitionApp.RegisterLink<CheckGameEndLink>("DayFlowController", "EndGameDecision", "EndGameState");
            AmbitionApp.RegisterLink("DayFlowController", "EndGameDecision", "NextDayState");
            AmbitionApp.RegisterLink("DayFlowController", "NextDayState", "UpdateCalendar");
        }
    }
}
