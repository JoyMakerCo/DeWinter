using System;
using System.Collections.Generic;
namespace Ambition
{
    public class LocationReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (reward.Value < 0)
            {
                int index = AmbitionApp.Paris.Daily == null ? -1 : Array.IndexOf(AmbitionApp.Paris.Daily, reward.ID);
                AmbitionApp.Paris.Exploration.RemoveAll(r => r == reward.ID);
                AmbitionApp.Paris.Rendezvous.RemoveAll(r => r == reward.ID);
                AmbitionApp.Paris.Completed.Add(reward.ID);
                if (index >= 0)
                {
                    List<string> daily = new List<string>();
                    for (int i=0; i< AmbitionApp.Paris.Daily.Length; ++i)
                    {
                        if (i != index)
                            daily.Add(AmbitionApp.Paris.Daily[i]);
                    }
                    AmbitionApp.Paris.Daily = daily.ToArray();
                }
            }
            else if (!AmbitionApp.Paris.Completed.Contains(reward.ID)
                && !AmbitionApp.Paris.Exploration.Contains(reward.ID)
                && !AmbitionApp.Paris.Rendezvous.Contains(reward.ID))
            {
                AmbitionApp.Paris.Exploration.Add(reward.ID);
            }
        }
    }
}
