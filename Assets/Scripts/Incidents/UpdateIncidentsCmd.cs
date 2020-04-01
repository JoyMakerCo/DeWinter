using Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ambition
{
    public class UpdateIncidentsCmd : ICommand
    {
        public void Execute()
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();

            IEnumerable<IncidentVO> unscheduled = calendar.FindUnscheduled<IncidentVO>(i =>
                (i.Requirements?.Length ?? 0) > 0
                && AmbitionApp.CheckRequirements((i as IncidentVO).Requirements));


            Debug.Log("Updating incidents");
            model.Reset();

            if (AmbitionApp.GetModel<InventoryModel>().CheckCaughtTrading())
			{
                model.IncidentQueue.Add( model.GettingCaughtIncident );
			}	
            model.IncidentQueue.AddRange(calendar.GetEvents<IncidentVO>());
            model.IncidentQueue.AddRange(unscheduled);
        }
    }
}
