using UnityEngine;
using UFlow;

namespace Ambition
{
    public class TutorialRemarkLink : ULink, Util.IInitializable, System.IDisposable
    {
        public override bool Validate() => false;

        public void Initialize()
        {
            AmbitionApp.Subscribe<RemarkVO>(Validate);
        }

        protected void Validate(RemarkVO value)
        {
            if (value != null) Activate();
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<RemarkVO>(Validate);
        }
    }
}
