using System;
using System.Collections.Generic;
namespace Ambition
{
    public class PostRendezvousState : UFlow.UState
    {
        public override void OnEnter()
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            foreach(IncidentVO incident in model.ExitIncidents)
            {
                if (AmbitionApp.CheckRequirements(incident.Requirements))
                {
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, incident);
                    break;
                }
            }
            model.Characters.TryGetValue(model.Rendezvous.Character, out CharacterVO character);
            if (character != null) character.LiaisonDay = -1;
            if (!AmbitionApp.Paris.Rendezvous.Contains(model.Rendezvous.Location))
            {
                AmbitionApp.Paris.Rendezvous.Add(model.Rendezvous.Location);
            }
            AmbitionApp.Paris.Exploration.Remove(model.Rendezvous.Location);
            AmbitionApp.SendMessage(GameMessages.ADD_EXHAUSTION);
        }
    }
}
