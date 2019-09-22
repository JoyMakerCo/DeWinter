using System;
using UFlow;
using Dialog;

namespace Ambition
{
	public class OpenDialogState : UState<string>
	{
        string dialogID;
        public override void SetData(string data) => dialogID = data;
        public override void OnEnterState(string[] args) => AmbitionApp.OpenDialog(dialogID);// args[0]);
    }
}
