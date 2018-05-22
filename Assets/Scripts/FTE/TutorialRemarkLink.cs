using UnityEngine;
using UFlow;

namespace Ambition
{
    public class TutorialRemarkLink : AmbitionValueLink<RemarkVO>
    {
        public override void Initialize()
        {
            ValidateOnCallback = r => { return r != null; };
            base.Initialize();
        }
    }
}
