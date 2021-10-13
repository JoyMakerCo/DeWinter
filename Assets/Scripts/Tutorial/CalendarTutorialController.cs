using System;
using UFlow;
namespace Ambition
{
    public class CalendarTutorialController : UFlowConfig
    {
        public override void Configure()
        {
            State("EstateTutorialStart");
            State("EstateTutorialInvitation");
            State("EstateTutorialInvitationInput");
            State("EstateTutorialInvitations");
            State("EstateTutorialInvitationsCompleteInput");
            State("EstateTutorialClickDate");
            State("EstateTutorialClickDateInput");
            State("EstateTutorialExit");

            Bind<TutorialState>("EstateTutorialStart");
            Bind<TutorialState>("EstateTutorialInvitation");
            Bind<TutorialState>("EstateTutorialInvitations");
            Bind<TutorialState>("EstateTutorialClickDate");
            Bind<EndTutorialState>("EstateTutorialExit");

            Bind<RSVPInput>("EstateTutorialInvitationInput");
            Bind<CloseRSVPInput>("EstateTutorialInvitationsCompleteInput");
            Bind<SelectDateInput>("EstateTutorialClickDateInput");
        }
    }

    public class SelectDateInput : UInput, Util.IInitializable, IDisposable
    {
        public void Initialize() => AmbitionApp.Subscribe<DateTime>(CalendarMessages.SELECT_DATE, HandleDate);
        public void Dispose() => AmbitionApp.Unsubscribe<DateTime>(CalendarMessages.SELECT_DATE, HandleDate);
        private void HandleDate(DateTime date) => Activate();
    }

    public class CloseRSVPInput : UInput, IDisposable
    {
        public CloseRSVPInput() : base() => AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, HandleClose);
        public void Dispose() => AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, HandleClose);
        private void HandleClose(string dialogID)
        {
            if (dialogID == DialogConsts.RSVP
                && !AmbitionApp.GetModel<PartyModel>().HasNewEvents(true, false)
                && !AmbitionApp.GetModel<CharacterModel>().HasNewEvents(true, false))
                Activate();
        }
    }

    public class RSVPInput : UInput, Util.IInitializable, IDisposable
    {
        public void Initialize()
        {
            AmbitionApp.Subscribe<PartyVO>(PartyMessages.ACCEPT_INVITATION, HandleRSVP);
            AmbitionApp.Subscribe<PartyVO>(PartyMessages.DECLINE_INVITATION, HandleRSVP);
            AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, HandleClose);
        }
        public void Dispose()
        {
            AmbitionApp.Unsubscribe<PartyVO>(PartyMessages.ACCEPT_INVITATION, HandleRSVP);
            AmbitionApp.Unsubscribe<PartyVO>(PartyMessages.DECLINE_INVITATION, HandleRSVP);
            AmbitionApp.Unsubscribe<string>(GameMessages.DIALOG_CLOSED, HandleClose);
        }
        private void HandleRSVP(PartyVO date) => Activate();
        private void HandleClose(string dialog) => Activate();
    }
}
