using System;
using UFlow;

namespace Ambition
{
    public class MessageInput : UInput, Util.IInitializable<string>, IDisposable
    {
        private string _event;

        public void Initialize(string data)
        {
            AmbitionApp.Subscribe(_event=data, Activate);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe(_event, Activate);
        }
    }
}
