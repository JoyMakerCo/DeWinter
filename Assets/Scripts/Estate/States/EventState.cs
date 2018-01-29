using System;
using UFlow;

namespace Ambition
{
	public class EventState : UState
	{
		public override void OnEnterState ()
		{
			EventModel model = AmbitionApp.GetModel<EventModel>();
			// Grant rewards
			foreach(RewardVO reward in model.Event.currentStage.Rewards)
			{
				AmbitionApp.SendMessage<RewardVO>(reward);
			}
			// Signal Event update
			AmbitionApp.SendMessage<EventVO>(model.Event);
		}
	}
}
