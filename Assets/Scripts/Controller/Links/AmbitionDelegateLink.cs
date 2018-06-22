using System;
using UFlow;

namespace Ambition
{
    public class AmbitionDelegateLink : ULink<string>
    {
        private string _event;
        override public void SetValue(string data)
        {
            AmbitionApp.Subscribe(_event = data, Activate);
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe(_event, Activate);
        }
    }
}
