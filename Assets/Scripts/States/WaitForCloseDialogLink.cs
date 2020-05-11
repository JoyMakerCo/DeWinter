using System;
using UFlow;

namespace Ambition
{
	public class WaitForCloseDialogLink : ULink, Util.IInitializable<string>, IDisposable
	{
        private string _dialogID;

        public void Initialize(string dialogID)
        {
            _dialogID = dialogID;
            AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, Handler);
        }
        public override bool Validate() => false;
        public void Dispose() => AmbitionApp.Unsubscribe<string>(GameMessages.DIALOG_CLOSED, Handler);
        private void Handler(string dialogID)
        {
            if (dialogID == _dialogID) Activate();
        }
    }
}
