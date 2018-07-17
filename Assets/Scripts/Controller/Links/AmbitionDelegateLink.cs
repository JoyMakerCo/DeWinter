using System;
using UFlow;

namespace Ambition
{
    public class AmbitionDelegateLink : ULink<string>
    {
        private string _event;
        override public void SetValue(string data)
        {
            _event = data;
        }

        public override void Initialize()
        {
            AmbitionApp.Subscribe(_event, Activate);
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe(_event, Activate);
        }
    }
}
