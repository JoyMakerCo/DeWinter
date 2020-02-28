using UFlow;

namespace Ambition
{
    public class SendMessageState : UState
    {
        protected string _message;
        public override void Initialize(object[] parameters)
        {
            _message = parameters[0] as string;
        }
        public override void OnEnterState()
        {
            AmbitionApp.SendMessage(_message);
        }
    }

    public class SendMessageState<M> : SendMessageState where M:class
    {
        private M _payload;
        public override void Initialize(object[] parameters)
        {
            base.Initialize(parameters);
            _payload = parameters.Length > 1 ? parameters[1] as M : null;
        }
        public override void OnEnterState()
        {
            AmbitionApp.SendMessage(_message, _payload);
        }
    }
}
