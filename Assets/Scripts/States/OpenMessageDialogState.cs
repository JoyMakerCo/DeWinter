using System;
using UFlow;
namespace Ambition
{
    public class OpenMessageDialogState : UState<string>
    {
        private string _dialogID;
        public override void SetData(string data) => _dialogID = data;
        public override void OnEnterState() => AmbitionApp.OpenMessageDialog(_dialogID);
    }
}
