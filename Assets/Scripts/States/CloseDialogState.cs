using System;
using UFlow;
using Dialog;

namespace Ambition
{
	public class CloseDialogState : UState, Util.IInitializable<string>
	{
		private string _dialogID;

		public void Initialize(string DialogID)
		{
			_dialogID = DialogID;
		}

		public override void OnEnterState ()
		{
            UnityEngine.Debug.Log("Close Dialog State - Entered!");
            AmbitionApp.CloseDialog(_dialogID);
		}
	}
}
