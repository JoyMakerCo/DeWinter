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
            foreach (string location in model.Locations)
            {
                if (!model.Locations.Contains(location)) // Todo: Check requirements
                {
                    model.Locations.Add(location);
                }
            }

            model.Locations.ForEach(l => AmbitionApp.SendMessage(ParisMessages.ADD_LOCATION, l));
            Array.ForEach(model.Daily, l => AmbitionApp.SendMessage(ParisMessages.ADD_LOCATION, l));
        }
    }
}
