using System;
using UFlow;
namespace Ambition
{
    public class LoadRendezvousLocationsState : UState, Core.IState
    {
        public override void OnEnter()
        {
            foreach (string loc in AmbitionApp.Paris.Rendezvous)
            {
                AmbitionApp.SendMessage(ParisMessages.SHOW_LOCATION, loc);
            }
        }
    }
}
