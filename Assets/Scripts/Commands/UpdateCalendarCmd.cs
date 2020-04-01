using System;
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
            GameModel model = AmbitionApp.GetModel<GameModel>();

            if (model.IsResting)
            {
                model.Exhaustion.Value = model.Exhaustion > 0 ? 0 : -1;
                model.IsResting = false;
            }

            for (int i = 0; i < 14; i++)
            {
                date = calendar.Today.AddDays(i);
                parties = calendar.GetEvents<PartyVO>();
                foreach (PartyVO party in parties)
                {
                    if (party.RSVP == RSVP.New)
                    {
                        AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
                    }
                }
            }

            // Kill off any scheduled events that don't satisfy requirements
            IncidentVO[] incidents = calendar.GetEvents<IncidentVO>();
            foreach (IncidentVO incident in incidents)
            {
                if (!AmbitionApp.CheckRequirements(incident.Requirements))
                {
                    calendar.Delete(incident);
                }
            }

            // Schedule all unscheduled incidents that have satisfied requirements
            incidents = calendar.FindUnscheduled<IncidentVO>(i => (i.Requirements != null && i.Requirements.Length > 0 && AmbitionApp.CheckRequirements(i.Requirements)) || i.Political);
            int chance = model.PoliticalChance;
            foreach(IncidentVO incident in incidents)
            {
                if (!incident.Political )
                {
                    calendar.Schedule(incident, calendar.Today);
                }
                else if (chance > 0 && Util.RNG.Generate(100) < chance && Array.IndexOf(incident.Chapters, model.Chapter) >= 0)
                {
                    calendar.Schedule(incident, calendar.Today);
                    chance = -1;
                }
            }
        }
    }
}
