using UnityEngine;
using UFlow;

namespace Ambition
{
    public class TutorialRemarkLink : ULink
    {
        public override bool InitializeAndValidate()
        {
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
            return false;
        }

        private void HandleRemark(RemarkVO remark)
        {
            if (remark != null) Validate();
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
        }
    }
}
