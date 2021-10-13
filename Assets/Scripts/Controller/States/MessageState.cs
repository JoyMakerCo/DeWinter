using System;
namespace Ambition
{
    public class MessageState : UFlow.UState, Util.IInitializable<string>
    {
        string _message;
        public void Initialize(string message) => _message = message;
        public override void OnEnter() => AmbitionApp.SendMessage(_message);
    }
}
