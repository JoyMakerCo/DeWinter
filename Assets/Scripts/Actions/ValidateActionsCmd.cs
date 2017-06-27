using System;
using System.Collections.Generic;
using Core;
using Ambition;

namespace Ambition
{
	public class ValidateActionsCmd : ICommand<string>
	{
		public void Execute(string type)
		{
			ActionModel model = AmbitionApp.GetModel<ActionModel>();
			List<ActionVO> actions;
			if (type != null && model.Actions.TryGetValue(type, out actions))
			{
				foreach (ActionVO action in actions)
				{
					if (ValidateAction(action))
					{
						DoAction(action);
						return;
					}
				}
			}
			if (model.Actions.TryGetValue(ActionConsts.DEFAULT, out actions))
			{
				foreach (ActionVO action in actions)
				{
					if (ValidateAction(action))
					{
						DoAction(action);
						return;
					}
				}
			}
		}

		private bool ValidateAction(ActionVO action)
		{
			return true;
		}

		private void DoAction(ActionVO action)
		{
			
		}
	}
}