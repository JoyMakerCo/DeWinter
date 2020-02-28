using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFlow;

namespace Ambition
{
	public class WaitForTutorialStepState : UInputState
	{
        public override void Initialize(object[] parameters)
        {
			AmbitionApp.Subscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleStep);
		}

		override public void Dispose()
		{
			AmbitionApp.Unsubscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleStep);
		}

		private void HandleStep(string step)
		{
			if (step == ID) Activate();
		}
	}
}
