using System;
using UFlow;
using Dialog;

namespace Ambition
{
	public class OpenDialogState : UState
	{
        private string _dialogID;
        public override void Initialize(object[] parameters)
        {
            _dialogID = parameters[0] as string;
        }

        public override void OnEnterState()
        {
            AmbitionApp.OpenDialog(_dialogID);
        }
    }
}
