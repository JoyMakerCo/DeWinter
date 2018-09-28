using UFlow;

namespace Ambition
{
    public class SendMessageState : UState<string>
    {
        private string _message;
        public override void SetData(string message) => _message = message;
        override public void OnEnterState() => AmbitionApp.SendMessage(_message);
    }
}
