using System;
using UFlow;

namespace Ambition
{
    public abstract class AmbitionValueLink<T> : ULink<T>
    {
        public Func<bool> ValidateOnInit=null;
        public Func<T, bool> ValidateOnCallback=null;
        public string MessageID=null;
        override public void Initialize()
        {
            if (ValidateOnInit != null && ValidateOnInit()) Activate();
            else if (ValidateOnCallback != null)
            {
                if (MessageID != null)
                    AmbitionApp.Subscribe<T>(MessageID, HandleValue);
                else
                    AmbitionApp.Subscribe<T>(HandleValue);
            }
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<T>(MessageID, HandleValue);
            AmbitionApp.Unsubscribe<T>(HandleValue);
        }
        
        public void HandleValue(T value)
        {
            if (ValidateOnCallback(value)) Activate();
        }
    }
}
