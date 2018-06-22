using UnityEngine;
using UFlow;

namespace Ambition
{
    public class TutorialRemarkLink : ULink
    {
        override public void Initialize()
        {
            AmbitionApp.Subscribe<RemarkVO>(Validate);
        }
        protected void Validate(RemarkVO value)
        {
            if (value != null) Activate();
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<RemarkVO>(Validate);
        }
    }
}
