using System;
using UFlow;
namespace Ambition
{
    public class OpenMessageDialogState : UState<string>
    {
        private string _phrase;

        public override void SetData(string data) => _phrase = data;
        public override void OnEnterState() => AmbitionApp.OpenMessageDialog(_phrase);
    }
}
