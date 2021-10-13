using System;
using System.Collections.Generic;

namespace Ambition
{
    public class SelectDailiesCmd : Core.ICommand<string[]>
    {
        // Chooses all of the new Daily locations whose requirements have been met
        // This ensures that the same locations aren't available on two consecutive days
        public void Execute(string[] pins)
        {
            ParisModel paris = AmbitionApp.Paris;
            if (paris.Daily == null
                && AmbitionApp.GetModel<CharacterModel>().Rendezvous == null
                && AmbitionApp.CheckRequirements(paris.ExploreParisRequirements))
            {
                GameModel game = AmbitionApp.Game;
                LocationVO location = null;
                List<string> subset = new List<string>();
                int num = paris.NumDailies;
                foreach (string pinID in pins)
                {
                    location = paris.GetLocation(pinID);
                    if (location != null
                        && location.IsDiscoverable
                        && !paris.Rendezvous.Contains(pinID)
                        && (!location.IsOneShot || !paris.Completed.Contains(pinID))
                        && AmbitionApp.CheckRequirements(location.Requirements))
                    {
                        if (!string.IsNullOrWhiteSpace(location.SceneID)
                            || AmbitionApp.CheckIncidentEligible(location.IntroIncident))
                        {
                            subset.Add(pinID);
                        }
                        else if (location.StoryIncidents != null)
                        {
                            foreach (string incidentID in location.StoryIncidents)
                            {
                                if (AmbitionApp.CheckIncidentEligible(incidentID))
                                {
                                    subset.Add(pinID);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (subset.Count > num)
                {
                    pins = Util.RNG.Shuffle(subset);
                    paris.Daily = new string[num];
                    for (int i = num - 1; i >= 0; i--)
                        paris.Daily[i] = pins[i];
                }
                else paris.Daily = subset.ToArray();
            }
        }
    }
}
