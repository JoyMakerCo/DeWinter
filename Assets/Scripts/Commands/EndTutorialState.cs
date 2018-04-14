using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Core;
using UFlow;

namespace Ambition
{
	public class EndTutorialState : TutorialState
	{
		public override void OnEnterState ()
		{
			base.OnEnterState();
			AmbitionApp.OpenMessageDialog("party_tutorial_end");

			AmbitionApp.UnregisterCommand<TutorialConfidenceCheckCmd>(PartyMessages.SHOW_MAP);
			AmbitionApp.RegisterCommand<OutOfConfidenceDialogCmd>(PartyMessages.SHOW_MAP);
		}
	}
}
