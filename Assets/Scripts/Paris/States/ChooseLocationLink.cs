using System;
using UFlow;

namespace Ambition
{
    public class ChooseLocationLink : ULink
    {
        public override void Initialize()
        {
            AmbitionApp.Subscribe<LocationPin>(HandlePin);
        }

        public override bool Validate() => false;

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe<LocationPin>(HandlePin);
        }

        void HandlePin(LocationPin pin)
        {
            AmbitionApp.GetModel<ParisModel>().Location = pin;
            Activate();
        }
    }
}
