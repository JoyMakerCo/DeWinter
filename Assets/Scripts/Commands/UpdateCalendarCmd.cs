﻿using System;
using System.Collections.Generic;
using Core;
namespace Ambition
{
    public class UpdateCalendarCmd : ICommand
    {
        public void Execute()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            PartyVO[] parties;
            DateTime date;
            AmbitionApp.GetModel<LocalizationModel>().SetDate(calendar.Today);

            for (int i = 0; i < 14; i++)
            {
                date = calendar.Today.AddDays(i);
                parties = calendar.GetEvents<PartyVO>();
                foreach(PartyVO party in parties)
                {
                    if (party.RSVP == RSVP.New)
                    {
                        AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
                    }
                }
            }

            // Kill off any scheduled events that don't satisfy requirements
            IncidentVO[] incidents = calendar.GetEvents<IncidentVO>();
            foreach(IncidentVO incident in incidents)
            {
                if (!AmbitionApp.CheckRequirements(incident.Requirements))
                {
                    calendar.Delete(incident);
                }
            }

            // Schedule all unscheduled incidents that have satisfied requirements
            incidents = calendar.FindUnscheduled<IncidentVO>(i => (i.Requirements?.Length ?? 0) > 0 && AmbitionApp.CheckRequirements(i.Requirements));
            Array.ForEach(incidents, i => calendar.Schedule(i, calendar.Today));
        }
    }
}
