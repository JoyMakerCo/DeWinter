using System;
namespace Ambition
{
    public class MessageInputState : UFlow.UInputState
    {
        private string _msg;
        public override void Initialize(object[] parameters)
        { 
            _msg = parameters[0] as string;
            AmbitionApp.Subscribe(_msg, Activate);
        }

        public override void Dispose() => AmbitionApp.Unsubscribe(_msg, Activate);
    }
}
