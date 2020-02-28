using System;
using Core;
using UFlow;
namespace Ambition
{
    public class RegisterDayFlowControllerCommand : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterState<SendMessageState>("DayFlowController", "InitCharacter", GameMessages.INIT_CHARACTER);
            AmbitionApp.RegisterState<SendMessageState>("DayFlowController", "InitUpdate", CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterState<SendMessageState>("DayFlowController", "UpdateCalendar", CalendarMessages.UPDATE_CALENDAR);
            AmbitionApp.RegisterState<LoadSceneState>("DayFlowController", "Chapter", SceneConsts.CHAPTER_PLACARD);
            AmbitionApp.RegisterState<MessageInputState>("DayFlowController", "ChapterInput", GameMessages.COMPLETE);
            AmbitionApp.RegisterState<LoadSceneState>("DayFlowController", "InitChapter", SceneConsts.CHAPTER_PLACARD);
            AmbitionApp.RegisterState<MessageInputState>("DayFlowController", "InitChapterInput", GameMessages.COMPLETE);
            AmbitionApp.RegisterState<LoadSceneState>("DayFlowController", "DayPlacard", SceneConsts.DAILY_PLACARD);
            AmbitionApp.RegisterState<UMachine>("DayFlowController", "StartIncident", "IncidentController");
            AmbitionApp.RegisterState("DayFlowController", "RequiredPartyDecision");
            AmbitionApp.RegisterState<UMachine>("DayFlowController", "Estate", "EstateController");
            AmbitionApp.RegisterState("DayFlowController", "PartyDecision");
            AmbitionApp.RegisterState<UMachine>("DayFlowController", "Party", "PartyController");
            AmbitionApp.RegisterState<UMachine>("DayFlowController", "Paris", "ParisMapController");
            AmbitionApp.RegisterState("DayFlowController", "LocationDecision");
            AmbitionApp.RegisterState("DayFlowController", "EndIncidentDecision");
            AmbitionApp.RegisterState<UMachine>("DayFlowController", "EndIncident", "IncidentController");
            AmbitionApp.RegisterState("DayFlowController", "EndGameDecision");
            AmbitionApp.RegisterState<SendMessageState>("DayFlowController", "NextDayState", CalendarMessages.NEXT_DAY);
            AmbitionApp.RegisterState<SendMessageState>("DayFlowController", "EndGameState", GameMessages.END_GAME);

            AmbitionApp.RegisterLink("DayFlowController", "InitCharacter", "InitUpdate");
            AmbitionApp.RegisterLink("DayFlowController", "InitUpdate", "InitChapter");
            AmbitionApp.RegisterLink("DayFlowController", "InitChapter", "InitChapterInput");
            AmbitionApp.RegisterLink("DayFlowController", "InitChapterInput", "StartIncident");
            AmbitionApp.RegisterLink<CheckChapterLink>("DayFlowController", "UpdateCalendar", "Chapter");
            AmbitionApp.RegisterLink("DayFlowController", "UpdateCalendar", "DayPlacard");
            AmbitionApp.RegisterLink("DayFlowController", "Chapter", "ChapterInput");
            AmbitionApp.RegisterLink("DayFlowController", "ChapterInput", "DayPlacard");

            AmbitionApp.RegisterLink("DayFlowController", "StartIncident", "RequiredPartyDecision");
            AmbitionApp.RegisterLink<CheckRequiredPartyLink>("DayFlowController", "RequiredPartyDecision", "Party");
            AmbitionApp.RegisterLink("DayFlowController", "RequiredPartyDecision", "Estate");
            AmbitionApp.RegisterLink("DayFlowController", "Estate", "PartyDecision");
            AmbitionApp.RegisterLink<CheckPartyLink>("DayFlowController", "PartyDecision", "Party");
            AmbitionApp.RegisterLink("DayFlowController", "PartyDecision", "Paris");
            AmbitionApp.RegisterLink("DayFlowController", "Paris", "LocationDecision");
            AmbitionApp.RegisterLink<CheckLocationLink>("DayFlowController", "Paris", "EndIncident");
            AmbitionApp.RegisterLink("DayFlowController", "Paris", "Estate");
            AmbitionApp.RegisterLink("DayFlowController", "Party", "EndIncident");
            AmbitionApp.RegisterLink("DayFlowController", "EndIncident", "EndGameDecision");
            AmbitionApp.RegisterLink<CheckGameEndLink>("DayFlowController", "EndGameDecision", "EndGameState");
            AmbitionApp.RegisterLink("DayFlowController", "EndGameDecision", "NextDayState");
            AmbitionApp.RegisterLink("DayFlowController", "NextDayState", "UpdateCalendar");
        }
    }
}
