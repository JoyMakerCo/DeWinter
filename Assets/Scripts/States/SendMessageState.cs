using UFlow;

namespace Ambition
{
    public class SendMessageState : UState, Util.IInitializable<string>
    {
        private string _message;
        public void Initialize(string message)
        {
            _message = message;
        }

        override public void OnEnterState()
        {
            AmbitionApp.SendMessage(_message);
        }
    }
}
