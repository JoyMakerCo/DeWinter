using System;
using System.Collections.Generic;
using Util;

namespace Ambition
{
    public class PopulateParisCmd : Core.ICommand
    {
        public void Execute()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            foreach (KeyValuePair<string, RequirementVO[]> location in model.Locations)
            {
                if (!model.Known.Contains(location.Key) && AmbitionApp.CheckRequirements(location.Value))
                {
                    model.Known.Add(location.Key);
                }
            }

            model.Known.ForEach(l => AmbitionApp.SendMessage(ParisMessages.ADD_LOCATION, l));
            model.Daily.ForEach(l => AmbitionApp.SendMessage(ParisMessages.ADD_LOCATION, l));
        }
    }
}
