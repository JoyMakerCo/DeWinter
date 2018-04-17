using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class ActionModel : DocumentModel
	{
		public Dictionary<string, List<ActionVO>> Actions;
		public List<ActionVO> History;

		private Dictionary<string, ActionVO> _definitions;

		ActionModel() : base("Actions")
		{
			Actions = new Dictionary<string, List<ActionVO>>();
		}

		public void AddAction(string actionName, bool topPriority=false)
		{
			ActionVO action;
			if (!_definitions.TryGetValue(actionName, out action))
				return;

			List<ActionVO> actions;
			if (!Actions.TryGetValue(action.Type, out actions))
				actions = new List<ActionVO>();

			if (topPriority)
				actions.Insert(0, action);
			else 
				actions.Add(action);

			Actions[action.Type] = actions;
		}
	}
}