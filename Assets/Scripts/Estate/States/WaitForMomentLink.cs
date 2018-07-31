using System;
using UFlow;

namespace Ambition
{
	public class WaitForMomentLink : ULink
	{
		private static float LAST_TIMESTAMP=0;

		public override void Initialize()
		{
			AmbitionApp.Subscribe<int>(IncidentMessages.INCIDENT_OPTION, HandleOption);
		}

		private void HandleOption(int option)
		{
			if (UnityEngine.Time.fixedTime == LAST_TIMESTAMP) return;
			LAST_TIMESTAMP = UnityEngine.Time.fixedTime;
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			MomentVO moment = model.Moment;
            IncidentVO incident = model.Incident;
			MomentVO target = null;
			TransitionVO transition = null;
			if (moment != null && incident != null)
			{
                MomentVO[] neighbors = incident.GetNeighbors(moment);
                if (option < neighbors.Length)
				{
                    TransitionVO[] transitions = incident.GetLinkData(moment);
                    transition = transitions[option];
                    target = neighbors[option];
				}

				// Grant rewards
				foreach(CommodityVO reward in moment.Rewards)
				{
					AmbitionApp.SendMessage<CommodityVO>(reward);
				}

				if (transition != null)
				{
					foreach(CommodityVO reward in transition.Rewards)
					{
						AmbitionApp.SendMessage<CommodityVO>(reward);
					}
				}
			}

			// Signal Event update
			model.Moment = target;
			Activate();
		}
		
		public override void Dispose ()
		{
			AmbitionApp.Unsubscribe<int>(HandleOption);
		}
	}
}
