using System;
using UFlow;

namespace Ambition
{
    public class AmbitionDelegateLink : ULink<string>
    {
        override public void Initialize()
        {
            AmbitionApp.Subscribe(Data, Activate);
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe(Data, Activate);
        }
    }
}
