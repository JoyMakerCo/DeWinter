using System;
using Core;
namespace Ambition
{
    public class ScheduleRendezvousCmd : ICommand<RendezVO>
    {
        public void Execute(RendezVO rendez)
        {
            RendezVO[] rendezVOs = AmbitionApp.Calendar.GetOccasions<RendezVO>(rendez.Day);
            if (Array.IndexOf(rendezVOs, rendez) < 0)
            {
                AmbitionApp.Calendar.Schedule(rendez, rendez.Day);
            }
            else
            {
                AmbitionApp.Calendar.Broadcast();
            }
        }
    }
}
