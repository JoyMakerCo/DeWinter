using System;
using UFlow;
namespace Ambition
{
    public class OpenMessageDialogState : UState, Util.IInitializable<string>
    {
        private string _dialogID;
        public void Initialize(string data) => _dialogID = data;
        public override void OnEnterState() => AmbitionApp.OpenMessageDialog(_dialogID);
    }
}
