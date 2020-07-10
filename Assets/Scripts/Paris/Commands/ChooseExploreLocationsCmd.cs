using System;
using System.Collections.Generic;

namespace Ambition
{
    public class ChooseExploreLocationsCmd : Core.ICommand<string[]>
    {
        // Chooses all of the new Daily locations whose requirements have been met
        // This ensures that the same locations aren't available on two consecutive days
        public void Execute(string[] pins)
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            List<string> dailies = new List<string>();
            if (model.Daily == null || model.Daily.Length < model.NumDailies)
            {
                dailies = model.Daily != null ? new List<string>(model.Daily) : new List<string>();
                Util.RNG.Shuffle(pins);
                for (int i = pins.Length - 1; dailies.Count < model.NumDailies && i >= 0; --i)
                {
                    if (!dailies.Contains(pins[i]))
                        dailies.Add(pins[i]);
                }
            }
            model.Daily = dailies.ToArray();
        }
    }
}
