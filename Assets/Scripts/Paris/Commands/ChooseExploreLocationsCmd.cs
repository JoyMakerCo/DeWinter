using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambition
{
    public class ChooseExploreLocationsCmd : Core.ICommand
    {
        // Chooses all of the new Daily locations whose requirements have been met
        // This ensures that the same locations aren't available on two consecutive days
        public void Execute()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            List<string> explorable =
                (from loc in model.Explorable
                where !model.Daily.Contains(loc.Key) && AmbitionApp.CheckRequirements(loc.Value)
                select loc.Key).ToList();
            if (explorable.Count <= model.NumExploreLocations)
            {
                model.Daily = explorable;
            }
            else
            {
                int index;
                model.Daily.Clear();
                for (int i= model.NumExploreLocations; i>0; i--)
                {
                    index = Util.RNG.Generate(explorable.Count);
                    model.Daily.Add(explorable[index]);
                    explorable.RemoveAt(index);
                }
            }
        }
    }
}
