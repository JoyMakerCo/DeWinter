﻿using System;
using UFlow;

namespace Ambition
{
	public class EndEventState : UState
	{
		public override void OnEnterState ()
		{
			EventModel model = AmbitionApp.GetModel<EventModel>();
			EventVO e = model.Config;
			if (e != null) AmbitionApp.SendMessage<EventVO>(EventMessages.END_EVENT, e);
			model.Config = null;
			model.Moment = null;
			AmbitionApp.CloseDialog(EventView.DIALOG_ID);
		}
	}
}
