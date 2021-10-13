using System;
using System.Collections.Generic;
using Core;
using UFlow;

namespace Ambition
{
    public class EstateFlow : UFlowConfig
    {
        private string _prev;

        public override void Configure()
        {
            State("ShowEstate");
            State("InitEstate");
            State("CheckInvitationsDecision");
            State("ShowInvitations");
            State("ShowInvitationsInput");

            State("CheckMissedParties",false);
            State("DisplayMissedParty");
            State("MissedPartyInput");

            State("CheckRendezvousResponses", false);
            State("DisplayRendezvousResponse");
            State("RendezvousResponseInput");

            State("Estate", false);
            State("LeaveEstateInput");
            State("Exit");

            State("RendezvousInput", false);
            State("RendezvousMap");
            State("LoadRendezvousLocations");
            State("RendezvousLocationInput");
            State("ReturnToEstate");

            State("ReturnToEstateInput", false);

            Link("Estate", "RendezvousInput");
            Link("ReturnToEstate", "Estate");
            Link("LoadRendezvousLocations", "ReturnToEstateInput");
            Link("ReturnToEstateInput", "ReturnToEstate");
            Link("CheckInvitationsDecision", "CheckMissedParties");
            Link("ShowInvitationsInput", "CheckInvitationsDecision");
            Link("MissedPartyInput", "CheckMissedParties");
            Link("CheckMissedParties", "CheckRendezvousResponses");
            Link("RendezvousResponseInput", "CheckRendezvousResponses");
            Link("CheckRendezvousResponses", "Estate");

            Decision("CheckInvitationsDecision", CheckInvitations);
            Decision("CheckMissedParties", CheckMissedEvents);
            Decision("CheckRendezvousResponses", CheckRendezvousResponses);

            Bind<LoadSceneInput, string>("ShowEstate", SceneConsts.ESTATE_SCENE);
            Bind<SetActivityState, ActivityType>("InitEstate", ActivityType.Estate);
            Bind<ShowInvitationsState>("ShowInvitations");
            Bind<CloseDialogInput, string>("ShowInvitationsInput", DialogConsts.RSVP);
            Bind<CloseDialogInput, string>("MissedPartyInput", DialogConsts.MESSAGE);
            Bind<CheckMissedPartiesState>("DisplayMissedParty");
            Bind<MessageInput, string>("LeaveEstateInput", EstateMessages.LEAVE_ESTATE);
            Bind<LoadRendezvousLocationsState>("LoadRendezvousLocations");
            Bind<MessageInput, string>("RendezvousInput", RendezvousMessages.CHOOSE_RENDEZVOUS);
            Bind<LoadSceneInput, string>("RendezvousMap", SceneConsts.PARIS_SCENE);
            Bind<MessageInput, string>("RendezvousLocationInput", ParisMessages.CHOOSE_LOCATION);
            Bind<MessageInput, string>("ReturnToEstateInput", ParisMessages.ESTATE);
            Bind<LoadSceneInput, string>("ReturnToEstate", SceneConsts.ESTATE_SCENE);
            Bind<CleanupEstateState>("Estate");
            Bind<CloseDialogInput, string>("RendezvousResponseInput", DialogConsts.RSVP);
            Bind<DisplayRendezvousResponseState>("DisplayRendezvousResponse");
        }

        private bool CheckInvitations()
        {
            return AmbitionApp.GetModel<PartyModel>().GetNewInvitations(true, false).Count > 0
                || AmbitionApp.GetModel<CharacterModel>().GetNewEvents(true, false).Count > 0;
        }

        private bool CheckMissedEvents()
        {
            CalendarModel cal = AmbitionApp.Calendar;
            PartyVO[] parties = cal.GetOccasions<PartyVO>(cal.Day - 1);
            if (Array.Exists(parties, p => p.RSVP == RSVP.New)) return true;
            RendezVO[] dates = cal.GetOccasions<RendezVO>(cal.Day - 1);
            return (Array.Exists(dates, r => r.RSVP == RSVP.New && !r.IsCaller));
        }

        private bool CheckRendezvousResponses()
        {
            CalendarModel cal = AmbitionApp.Calendar;
            CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
            List<RendezVO> results = characters.GetPendingInvitations(true, false);
            return results.Count > 0;
        }
    }
}
