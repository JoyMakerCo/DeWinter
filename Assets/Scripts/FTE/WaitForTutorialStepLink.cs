using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFlow;

namespace Ambition
{
	public class WaitForTutorialStepLink : ULink
	{
		override public bool InitializeAndValidate()
		{
			
			if (!AmbitionApp.IsActiveState(TutorialConsts.TUTORIAL)) return true;
			AmbitionApp.Subscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleStepComplete);
			return false;
		}

		private void HandleStepComplete(string step)
		{
			if (step == State) 
			{
				Dispose();
				Validate();
			}
		}

		override public void Dispose()
		{
			AmbitionApp.Unsubscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleStepComplete);
		}
	}
}
