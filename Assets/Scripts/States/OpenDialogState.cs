using System;
using UFlow;
using Dialog;

namespace Ambition
{
	public class OpenDialogState : UState, Util.IInitializable<string>, Core.IState
    {
        string dialogID;
        public void Initialize(string data) => dialogID = data;
        public override void OnEnter() => AmbitionApp.OpenDialog(dialogID);// args[0]);
    }
}
