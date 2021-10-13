using System;
using System.Collections.Generic;
namespace Ambition
{
    public class EveningState : UFlow.UState
    {
        public override void OnEnter()
        {
            AmbitionApp.Game.Activity = ActivityType.Evening;
            IncidentModel story = AmbitionApp.Story;
            GossipModel gossip = AmbitionApp.Gossip;
            IncidentVO[] incidents;
            IncidentVO incident;
            if (gossip.GossipActivity > 0 && Util.RNG.Generate(100) < gossip.ReproachChance[gossip.GossipActivity-1])
            {
                incidents = AmbitionApp.Story.GetIncidents(IncidentType.Caught);
                incident = TakeRandom(incidents);
                if (incident != null)
                {
                    incident.Date = AmbitionApp.Calendar.Today.AddDays(1);
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, incident);
                }
            }
            gossip.GossipActivity = 0;
            if (AmbitionApp.Game.Perilous)
            {
                incidents = story.GetIncidents(IncidentType.Peril);
                incident = TakeRandom(incidents);
                if (incident != null)
                {
                    incident.Date = AmbitionApp.Calendar.Today;
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, incident);
                }
            }
            AmbitionApp.Game.Perilous = false;
        }

        IncidentVO TakeRandom(IncidentVO[] incidents)
        {
            List<IncidentVO> eligible = new List<IncidentVO>();
            foreach(IncidentVO incident in incidents)
            {
                if (AmbitionApp.CheckIncidentEligible(incident))
                {
                    eligible.Add(incident);
                }
            }
            return Util.RNG.TakeRandom(eligible);
        }
    }
}
