using System;
using System.Linq;
using System.Collections.Generic;

namespace Ambition
{
    public class PopulateParisCmd : Core.ICommand
    {
        public void Execute()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            model.Visited.ForEach(l => model.Locations.Remove(l));
            IEnumerable<string> newLocations =
                (from location in model.Locations
                 where !model.Known.Contains(location.Key) && AmbitionApp.CheckRequirements(location.Value)
                 select location.Key);
            model.Known.AddRange(newLocations);
            model.Known.ForEach(l => AmbitionApp.SendMessage(ParisMessages.ADD_LOCATION, l));
            model.Daily.ForEach(l => AmbitionApp.SendMessage(ParisMessages.ADD_LOCATION, l));
        }
    }
}
