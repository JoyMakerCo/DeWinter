using System;
using UFlow;
using Dialog;

namespace Ambition
{
	public class OpenDialogState : UState<string>
	{
		private string _dialogID;
		override public void SetData(string dialogID)
		{
			_dialogID = dialogID;
		}

		public override void OnEnterState ()
		{
            AmbitionApp.OpenDialog(_dialogID);
		}
	}
}
