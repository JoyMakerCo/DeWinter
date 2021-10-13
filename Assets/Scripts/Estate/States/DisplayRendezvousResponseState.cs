using System;
using System.Collections.Generic;
namespace Ambition
{
    public class DisplayRendezvousResponseState : UFlow.UState
    {
        public override void OnEnter()
        {
            CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
            List<RendezVO> rendezs = characters.GetPendingInvitations(true, false);
            if (rendezs.Count > 0)
            {
                RendezVO rendez = rendezs[0];
                AmbitionApp.SendMessage(RendezvousMessages.CREATE_RENDEZVOUS_RESPONSE, rendez);
                AmbitionApp.OpenDialog(DialogConsts.RSVP, new CalendarEvent[] { rendez });
            }
        }
    }
}
