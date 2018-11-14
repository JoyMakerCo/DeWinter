using System;
using UFlow;

namespace Ambition
{
    public class ChooseLocationLink : ULink
    {
        public override void Initialize()
        {
            AmbitionApp.Subscribe<LocationPin>(ParisMessages.GO_TO_LOCATION, HandlePin);
        }

        public override bool Validate() => false;

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe<LocationPin>(ParisMessages.GO_TO_LOCATION, HandlePin);
        }

        void HandlePin(LocationPin pin)
        {
            AmbitionApp.GetModel<ParisModel>().Location = pin;
            if (pin != null) Activate();
        }
    }
}
