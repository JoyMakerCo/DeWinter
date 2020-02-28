using System;
using UFlow;
namespace Ambition
{
    public class OpenMessageDialogState : UState
    {
        private string _message;
        public override void Initialize(object[] parameters)
        {
            _message = parameters[0] as string;
        }
        public override void OnEnterState() => AmbitionApp.OpenMessageDialog(_message);
    }
}
