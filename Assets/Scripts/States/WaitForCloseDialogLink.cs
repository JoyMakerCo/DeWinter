using System;
using UFlow;

namespace Ambition
{
	public class WaitForCloseDialogLink : ULink<string>
	{
        private string _dialogID;
        override public void SetValue(string dialogID) => _dialogID = dialogID;
        public override bool Validate() => false;
        public override void Initialize() => AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, Handler);
        public override void Dispose() => AmbitionApp.Unsubscribe<string>(GameMessages.DIALOG_CLOSED, Handler);
        private void Handler(string dialogID)
        {
            if (dialogID == _dialogID) Activate();
        }
    }
}
