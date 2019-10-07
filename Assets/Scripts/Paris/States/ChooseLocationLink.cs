using System;
using UFlow;

namespace Ambition
{
    public class ChooseLocationLink : ULink
    {
        public override void Initialize() => AmbitionApp.Subscribe<LocationVO>(ParisMessages.GO_TO_LOCATION, HandlePin);
        public override bool Validate() => false;
        public override void Dispose() => AmbitionApp.Unsubscribe<LocationVO>(ParisMessages.GO_TO_LOCATION, HandlePin);

        void HandlePin(LocationVO location)
        {
            if (location != null) Activate();
        }
    }
}
