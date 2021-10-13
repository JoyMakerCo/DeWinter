using System;
using Core;
using UFlow;
namespace Ambition
{
    public class DayFlow : UFlowConfig
    {
        public override void Configure()
        {
            State("UpdateCalendar");
            State("SetActivity");
            State("Chapter");
            State("ChapterInput");
            State("MorningIncident");
            State("EndGameDecision");
            State("Roll Credits");

            State("Estate",false);
            State("PartyDecision");
            State("Party");
            State("Evening");
            State("EveningIncident");
            State("NextDayState");

            State("RendezvousDecision",false);
            State("Rendezvous");
            State("Paris",false);
            State("EveningDecision");

            Link("EndGameDecision", "Estate");
            Link("PartyDecision", "RendezvousDecision");
            Link("RendezvousDecision", "Paris");
            Link("EveningDecision", "Evening");
            Link("EveningDecision", "Estate");
            Link("Rendezvous", "Evening");
            Link("NextDayState", "UpdateCalendar");

            Decision("EveningDecision", ()=>AmbitionApp.Paris.Location != null);
            Decision("PartyDecision", IsAttendingParty);
            Decision("RendezvousDecision", IsAttendingRendezvous);
            Decision("EndGameDecision", IsEndGame);

            Bind<AdvanceDayState>("NextDayState");
            Bind<UpdateCalendarState>("UpdateCalendar");
            Bind<SetActivityState, ActivityType>("SetActivity", ActivityType.Estate);
            Bind<UMachine>("MorningIncident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<UMachine>("Estate", FlowConsts.ESTATE_CONTROLLER);
            Bind<UMachine>("Party", FlowConsts.PARTY_CONTROLLER);
            Bind<UMachine>("Paris", FlowConsts.PARIS_CONTROLLER);
            Bind<UMachine>("Rendezvous", FlowConsts.RENDEZVOUS_CONTROLLER);
            Bind<EveningState>("Evening");
            Bind<UMachine>("EveningIncident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<LoadSceneInput, string>("Chapter", SceneConsts.CHAPTER_PLACARD);
            Bind<MessageInput, string>("ChapterInput", GameMessages.COMPLETE);
            Bind<LoadSceneInput, string>("Roll Credits", SceneConsts.CREDITS);
        }

        private bool IsAttending<T>() where T:CalendarEvent
        {
            T[] occasions = AmbitionApp.Calendar.GetOccasions<T>();
            return occasions != null ? Array.Exists(occasions, t => t.IsAttending) : false;
        }
        private bool IsAttendingParty() => IsAttending<PartyVO>();
        private bool IsAttendingRendezvous() => IsAttending<RendezVO>();
        private bool IsEndGame() => AmbitionApp.Story.IsComplete(SceneConsts.EPILOGUE_SCENE, false);
    }
}
