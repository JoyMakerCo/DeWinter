using System.Collections.Generic;
using Core;

namespace Actions
{
	public class ActionModel : IModel
	{
		public List<ActionVO> Actions;
		public List<ActionVO> History;

		ActionModel()
		{
			Actions = new List<ActionVO>();
		}

		public void AddAction(ActionVO action, bool topPriority=false)
		{
			if (topPriority)
				Actions.Insert(0, action);
			else 
				Actions.Add(action);
		}
	}
}