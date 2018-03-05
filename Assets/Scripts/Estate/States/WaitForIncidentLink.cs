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
			TransitionVO link;
			if (moment != null && config != null)
			{
				int index = Array.IndexOf(config.Moments, model.Moment);
				TransitionVO[] links = Array.FindAll(model.Config.Transitions, l=>l.Index == index);
				if (option < links.Length)
				{
					link = links[option];
					index = link.Target;
					if (index < config.Moments.Length) target = config.Moments[index];
				}

				// Grant rewards
				foreach(CommodityVO reward in moment.Rewards)
				{
					AmbitionApp.SendMessage<CommodityVO>(reward);
				}

				// foreach(RewardVO reward in link.Rewards)
				// {
				// 	AmbitionApp.SendMessage<RewardVO>(reward);
				// }
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
