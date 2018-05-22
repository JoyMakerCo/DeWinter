using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFlow;

namespace Ambition
{
	public class WaitForTutorialStepLink : AmbitionValueLink<string>
	{
		override public void Initialize()
		{
			MessageID = TutorialMessage.TUTORIAL_STEP_COMPLETE;
			ValidateOnCallback = s => { return s == Data; };
			base.Initialize();
		}
	}
}
