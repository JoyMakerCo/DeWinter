using System;
using System.Collections.Generic;

namespace Ambition
{
    public class ChooseExploreLocationsCmd : Core.ICommand<Pin[]>
    {
        // Chooses all of the new Daily locations whose requirements have been met
        // This ensures that the same locations aren't available on two consecutive days
        public void Execute(Pin[] pins)
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            int length = pins.Length;
            uint max = model.NumDailies;
            int[] random = new int[length];

            // Shuffle the pin indices (stop short of shuffling within the results)
            for (int i = length - 1; i >= max; i--)
            {
                random[i] = Util.RNG.Generate(i);
                random[random[i]] = i;
            }
            model.Daily = new string[max];
            // Build a list from the first N shiffled indices
            for (int i = 0; i < max; i++)
            {
                model.Daily[i] = pins[random[i]].name;
            }
        }
    }
}
