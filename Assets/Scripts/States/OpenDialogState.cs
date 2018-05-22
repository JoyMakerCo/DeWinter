using System;
using UFlow;
using Dialog;

namespace Ambition
{
	public class OpenDialogState : UState<string>
	{
		public void Initialize (string DialogID)
		{
			Data = DialogID;
		}

		public override void OnEnterState ()
		{
			AmbitionApp.OpenDialog(Data);
		}
	}
}
