using System;
using Core;
using UFlow;

namespace Ambition
{
	public class StartEventState : UState
	{
		private const string MODAL_ID = "EVENT";

		public override void OnEnterState ()
		{
			EventModel model = AmbitionApp.GetModel<EventModel>();
			model.Event.currentStageIndex = 0;
			AmbitionApp.OpenDialog(MODAL_ID);
	    }
	}
}
