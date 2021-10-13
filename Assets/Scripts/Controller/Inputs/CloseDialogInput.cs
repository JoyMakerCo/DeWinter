using System;
namespace Ambition
{
    public class CloseDialogInput : UFlow.UInput, Util.IInitializable<string>, IDisposable
    {
        private string _dialogID;
        public void Initialize(string dialogID)
        {
            _dialogID = dialogID;
            AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, HandleDialog);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<string>(GameMessages.DIALOG_CLOSED, HandleDialog);
        }

        private void HandleDialog(string dialogID)
        {
            if (dialogID == _dialogID) Activate();
        }
    }
}
