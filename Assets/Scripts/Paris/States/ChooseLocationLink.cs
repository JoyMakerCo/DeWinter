using System;
using UFlow;

namespace Ambition
{
    public class ChooseLocationLink : ULink
    {
        public override void Initialize() => AmbitionApp.Subscribe<Pin>(ParisMessages.GO_TO_LOCATION, HandlePin);
        public override bool Validate() => false;
        public override void Dispose() => AmbitionApp.Unsubscribe<Pin>(ParisMessages.GO_TO_LOCATION, HandlePin);

        void HandlePin(Pin pin)
        {
            if (pin != null) Activate();
        }
    }
}
