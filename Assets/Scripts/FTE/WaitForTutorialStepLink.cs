using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFlow;

namespace Ambition
{
	public class WaitForTutorialStepLink : ULink, Util.IInitializable, System.IDisposable
	{
		protected string _stepID;

        public override bool Validate() => false;

        public void Initialize ()
		{
//			_stepID = _machine._graph.Nodes[_origin].ID;
			AmbitionApp.Subscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleStep);
		}

		public void Dispose()
		{
			AmbitionApp.Unsubscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleStep);
		}

		private void HandleStep(string step)
		{
			if (step == _stepID) Activate();
		}
	}
}
