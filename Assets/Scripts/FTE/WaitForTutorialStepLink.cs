using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFlow;

namespace Ambition
{
	public class WaitForTutorialStepLink : ULink
	{
		protected string _stepID;
		override public void Initialize ()
		{
			_stepID = _machine._graph.Nodes[_origin].ID;
			AmbitionApp.Subscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleStep);
		}

		override public void Dispose()
		{
			AmbitionApp.Unsubscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleStep);
		}

		private void HandleStep(string step)
		{
			if (step == _stepID) Activate();
		}
	}
}
