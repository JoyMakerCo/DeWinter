using System;
using UFlow;

namespace Ambition
{
    public class MessageLink : ULink, Util.IInitializable<string>, IDisposable
    {
        private string _event;
        public void Initialize(string data)
        {
            _event = data;
            AmbitionApp.Subscribe(_event, Activate);
        }

        public override bool Validate() => false;
        public void Dispose() => AmbitionApp.Unsubscribe(_event, Activate);
    }
}
