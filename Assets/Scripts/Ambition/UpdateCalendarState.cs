using System;
using System.Collections.Generic;
using UFlow;
namespace Ambition
{
    public class UpdateCalendarState : UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            OccasionVO[] occasions;
            DateTime date;
            AmbitionApp.GetModel<LocalizationModel>().SetDate(calendar.Today);
            GameModel model = AmbitionApp.GetModel<GameModel>();
            PartyModel parties = AmbitionApp.GetModel<PartyModel>();
            PartyVO party;
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();

            model.Activity = ActivityType.Estate;
            if (model.IsResting)
            {
                model.Exhaustion.Value = model.Exhaustion > 0 ? 0 : -1;
                model.IsResting = false;
            }

            for (int i = 0; i < 14; i++)
            {
                occasions = calendar.GetOccasions(OccasionType.Party, calendar.Day + i);
                foreach (OccasionVO occasion in occasions)
                {
                    if (parties.Parties.TryGetValue(occasion.ID, out party) && party.RSVP == RSVP.New)
                    {
                        AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
                    }
                }
            }

            // Schedule all unscheduled incidents that have satisfied requirements

/*            incidents = new List<IncidentVO>(incidentModel.Incidents.Values);
            int chance = model.PoliticalChance;
            foreach (IncidentVO incident in incidents)
            {
                if (!incident.Political)
                {
                    incident.Date = calendar.Today;
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, incident);
                }
                else if (chance > 0 && Util.RNG.Generate(100) < chance && Array.IndexOf(incident.Chapters, model.Chapter) >= 0)
                {
                    calendar.Schedule(incident, calendar.Today);
                    chance = -1;
                }
            }

          .FindUnscheduled<IncidentVO>(i => ((i as IncidentVO)?.Requirements != null && ((IncidentVO)i).Requirements.Length > 0 && AmbitionApp.CheckRequirements(((IncidentVO)i).Requirements)) || ((IncidentVO)i).Political);
          */
        }
    }
}
