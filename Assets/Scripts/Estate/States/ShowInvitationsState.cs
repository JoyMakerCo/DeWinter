using System;
using System.Collections.Generic;
namespace Ambition
{
    public class ShowInvitationsState : UFlow.UState
    {
        public override void OnEnter()
        {
            List<CalendarEvent> events = null;
            List<PartyVO> parties = AmbitionApp.GetModel<PartyModel>().GetNewInvitations(true, false);
            List<RendezVO> rendezs;
            if (parties.Count == 0)
            {
                CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
                rendezs = model.GetNewEvents(true, false);
                if (rendezs.Count > 0)
                {
                    events = rendezs.ConvertAll(r => (CalendarEvent)r);
                    events.Sort();
                    events = events.FindAll(e => e.Day == events[0].Day);
                }
            }
            else
            {
                parties.Sort();
                events = parties.FindAll(p=>p.Day == parties[0].Day).ConvertAll(p => (CalendarEvent)p);
                RendezVO[] occasions = AmbitionApp.Calendar.GetOccasions<RendezVO>(parties[0].Day);
                occasions = Array.FindAll(occasions, r => r.RSVP == RSVP.New);
                events.AddRange(occasions);
            }
            events?.ForEach(e => e.Created = AmbitionApp.Calendar.Day);
            AmbitionApp.OpenDialog(DialogConsts.RSVP, events?.ToArray() ?? new CalendarEvent[0]);
        }
    }
}
