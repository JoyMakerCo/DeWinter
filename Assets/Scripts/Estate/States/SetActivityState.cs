using System;
using UFlow;
namespace Ambition
{
    public class SetActivityState : UState, Util.IInitializable<ActivityType>
    {
        public void Initialize(ActivityType type)
        {
            AmbitionApp.GetModel<GameModel>().Activity = type;
        }
    }
}
