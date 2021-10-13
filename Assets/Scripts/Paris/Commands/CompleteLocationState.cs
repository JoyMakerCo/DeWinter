using System;
namespace Ambition
{
    public class CompleteLocationState : UFlow.UState
    {
        public override void OnEnter()
        {
            ParisModel paris = AmbitionApp.Paris;
            string location = paris.Location;
            if (!string.IsNullOrEmpty(location))
            {
                LocationVO vo = AmbitionApp.Paris.GetLocation();
                bool completed = vo.IsOneShot;
                if (completed)
                {
                    if (!paris.Completed.Contains(location))
                        paris.Completed.Add(location);
                    paris.Exploration.Remove(location);
                    paris.Rendezvous.Remove(location);
                }
                else completed = paris.Completed.Contains(location);
                if (!completed)
                {
                    if (vo.IsRendezvous)
                    {
                        paris.Exploration.Remove(location);
                        paris.Rendezvous.Add(location);
                    }
                    else if (!AmbitionApp.Paris.Exploration.Contains(location))
                    {
                        paris.Rendezvous.Remove(location);
                        paris.Exploration.Add(location);
                    }
                }
            }
        }
    }
}
