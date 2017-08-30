using System;
using UFlow;
using Dialog;

namespace Ambition
{
	public class OpenDialogState : UState, Util.IInitializable<string>
	{
		public void Initialize (string DialogID)
		{
			AmbitionApp.OpenDialog(DialogID);
		}

		public override void OnEnterState ()
		{
			End();
		}
	}
}

