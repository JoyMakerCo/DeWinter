using System;
using UFlow;

namespace Ambition
{
	public class WaitForCloseDialogTransition : UTransition
	{
		public override bool InitializeAndValidate ()
		{
			AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, HandleDialogClosed);
			return false;
		}

		private void HandleDialogClosed(string DialogID)
		{
			if (DialogID == Parameters[0] as String)
			{
				Validate();
			}
		}
	}
}

