using System;
using UFlow;
using Dialog;

namespace Ambition
{
	public class OpenDialogState : UState<string>
	{
        string dialogID;
        public override void SetData(string data) => dialogID = data;
        public override void OnEnterState() => AmbitionApp.OpenDialog(dialogID);// args[0]);
    }
}
