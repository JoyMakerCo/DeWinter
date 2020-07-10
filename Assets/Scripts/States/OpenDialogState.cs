using System;
using UFlow;
using Dialog;

namespace Ambition
{
	public class OpenDialogState : UState, Util.IInitializable<string>
	{
        string dialogID;
        public void Initialize(string data) => dialogID = data;
        public override void OnEnterState() => AmbitionApp.OpenDialog(dialogID);// args[0]);
    }
}
