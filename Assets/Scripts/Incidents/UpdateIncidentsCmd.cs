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
/*
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();

            IEnumerable<IncidentVO> unscheduled = calendar.FindUnscheduled<IncidentVO>(i =>
                ((i as IncidentVO)?.Requirements?.Length ?? 0) > 0
                && AmbitionApp.CheckRequirements(((IncidentVO)i).Requirements));


            Debug.Log("Updating incidents");

            if (AmbitionApp.GetModel<InventoryModel>().CheckCaughtTrading())
			{
                calendar.Schedule(model.GettingCaughtIncident, calendar.Today);
			}	
            foreach(IncidentVO incident in unscheduled)
            {
                calendar.Schedule(incident, calendar.Today);
            }
            */
        }
    }
}
