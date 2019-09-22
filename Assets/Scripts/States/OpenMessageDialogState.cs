using System;
using UFlow;
namespace Ambition
{
    public class OpenMessageDialogState : UState
    {
        public override void OnEnterState(string[] args) => AmbitionApp.OpenMessageDialog(args[0]);
    }
}
