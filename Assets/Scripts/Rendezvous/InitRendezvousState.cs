using System;
namespace Ambition
{
    public class InitRendezvousState : UFlow.UState
    {
        public override void OnEnter()
        {
            RendezVO vous = AmbitionApp.Calendar.GetOccasions<RendezVO>()[0];
            AmbitionApp.GetModel<GameModel>().Activity = ActivityType.Rendezvous;
            LocationVO location = AmbitionApp.Paris.GetLocation(vous.Location);
            IncidentVO incident;

            // This will crash if the rendezvous location is badly configured
            foreach (string ID in location.StoryIncidents)
            {
                AmbitionApp.Story.Incidents.TryGetValue(ID, out incident);
                if (incident != null && AmbitionApp.CheckIncidentEligible(incident))
                {
                    AmbitionApp.Story.Schedule(incident);
                    return;
                }
            }

            // TODO: Evaluate Outfit for Rendezvous
        }
    }
}
