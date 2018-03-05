using System;
using UFlow;

namespace Ambition
{
	public class WaitForIncidentLink : ULink
	{
		public override bool InitializeAndValidate ()
		{
			AmbitionApp.Subscribe<int>(IncidentMessages.INCIDENT_OPTION, HandleOption);
			return false;
		}

		private void HandleOption(int option)
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			MomentVO moment = model.Moment;
			IncidentVO config = model.Config;
			MomentVO target = null;
			TransitionVO transition = null;
			if (moment != null && config != null)
			{
				int index = Array.IndexOf(config.Moments, model.Moment);
				TransitionVO[] links = Array.FindAll(model.Config.Transitions, l=>l.Index == index);
				if (option < links.Length)
				{
					transition = links[option];
					index = transition.Target;
					if (index < config.Moments.Length) target = config.Moments[index];
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
			if (target != null) Validate();
		}
		
		public override void Dispose ()
		{
			AmbitionApp.Unsubscribe<int>(HandleOption);
		}
	}
}
