using System;
using Core;
using UnityEngine;

namespace Ambition
{
    public class ExploreParisCmd : ICommand<LocationVO[]>
    {
        private const int DISCOVER_CHANCE = 50;

        public void Execute(LocationVO[] locations)
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            foreach (LocationVO location in locations)
            {
                if (!paris.VisitedLocations.ContainsKey(location.Name) && Util.RNG.Generate(100) < DISCOVER_CHANCE)
                {
                    AmbitionApp.SendMessage<string>(ParisMessages.ADD_LOCATION, location.Name);
                    paris.Locations.Add(location.Name, location);
                    if (location.IncidentID != null)
                    {
                        AmbitionApp.SendMessage<string>(IncidentMessages.START_INCIDENT, location.IncidentID);
                    }
                    return;
                }
            }
        }
    }
}
