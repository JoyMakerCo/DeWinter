using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class SelectDateCmd : ICommand<DateTime>
	{
		public void Execute (DateTime date)
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO[] parties = calendar.GetOccasions<PartyVO>(date);
            RendezVO[] dates = calendar.GetOccasions<RendezVO>(date);
            if (parties.Length + dates.Length > 0)
            {
                PartyVO party = Array.Find(parties, p => p.IsAttending);
                RendezVO rendez = Array.Find(dates, p => p.IsAttending);
                if (party != null)
                {
                    AmbitionApp.OpenDialog(DialogConsts.RSVP, new CalendarEvent[] { party });
                }
                else if (rendez != null)
                {
                    AmbitionApp.OpenDialog(DialogConsts.RSVP, new CalendarEvent[] { rendez });
                }
                else
                {
                    List<CalendarEvent> events = new List<CalendarEvent>(parties);
                    events.AddRange(dates);
                    AmbitionApp.OpenDialog(DialogConsts.RSVP, events.ToArray());
                }
            }
            else if (date.Subtract(calendar.Today).Days >= 1 && AmbitionApp.Paris.Rendezvous.Count > 0)
            {
                CharacterModel cModel = AmbitionApp.GetModel<CharacterModel>();
                foreach (CharacterVO character in cModel.Characters.Values)
                {
                    if (character.IsDateable && !character.IsRendezvousScheduled)
                    {
                        AmbitionApp.OpenDialog(DialogConsts.CREATE_RENDEZVOUS, date);
                        break;
                    }
                }
            }
		}
    }
}
