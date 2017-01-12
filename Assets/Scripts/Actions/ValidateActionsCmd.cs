using System;
using System.Collections.Generic;
using Core;

namespace Actions
{
	public class ValidateActionsCmd : ICommand
	{
		public void Execute()
		{
			ActionModel model = App.Service<ModelSvc>().GetModel<ActionModel>();
			foreach (List<ActionVO> kvp in model.
		}

		private bool ValidateAction(ActionVO action)
		{
			
		}
	}
}