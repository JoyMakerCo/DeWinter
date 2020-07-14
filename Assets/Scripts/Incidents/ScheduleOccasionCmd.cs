using Core;
using System;

namespace Ambition
{
    public class ScheduleOccasionCmd : ICommand<OccasionVO>
    {
        public void Execute(OccasionVO occasion)
        {
            AmbitionApp.GetModel<CalendarModel>().Schedule(occasion);
        }
    }

    public class ScheduleIncidentCmd : ICommand<IncidentVO>
    {
        public void Execute(IncidentVO incident)
        {
            AmbitionApp.GetModel<IncidentModel>().Schedule(incident);
        }
    }

    public class SchedulePartyCmd : ICommand<PartyVO>
    {
        public void Execute(PartyVO party)
        {
            if (party?.Name != null)
            {
                PartyModel model = AmbitionApp.GetModel<PartyModel>();
                CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                if (party.RSVP != RSVP.Required) party.RSVP = RSVP.Accepted;
                model.Parties[party.Name] = party;
                if (!party.Date.Equals(default))
                {
                    OccasionVO occasion = new OccasionVO()
                    {
                        Day = party.Date.Subtract(calendar.StartDate).Days,
                        Type = OccasionType.Party,
                        ID = party.ID,
                        IsComplete = false
                    };
                    AmbitionApp.GetModel<CalendarModel>().Schedule(occasion);
                }
            }
        }
    }
}
