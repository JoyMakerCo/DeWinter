using System;
using UFlow;

namespace Ambition
{
	public class WaitForEventTransition : UTransition
	{
		public override bool InitializeAndValidate ()
		{
			AmbitionApp.Subscribe<int>(EventMessages.EVENT_OPTION, HandleOption);
			return false;
		}

		private void HandleOption(int option)
		{
			EventModel model = AmbitionApp.GetModel<EventModel>();
			MomentVO moment = model.Moment;
			EventVO config = model.Config;
			MomentVO target = null;
			EventConfigLinkVO link;
			if (moment != null && config != null)
			{
				int index = Array.IndexOf(config.Moments, model.Moment);
				EventConfigLinkVO[] links = Array.FindAll(model.Config.Links, l=>l.Index == index);
				if (option < links.Length)
				{
					link = links[option];
					index = link.Target;
					if (index < config.Moments.Length) target = config.Moments[index];
				}

				// Grant rewards
				foreach(RewardVO reward in moment.Rewards)
				{
					AmbitionApp.SendMessage<RewardVO>(reward);
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
