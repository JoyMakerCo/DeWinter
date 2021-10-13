using System;
namespace Ambition
{
    public class MiscReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward) => AmbitionApp.Game.Misc[reward.ID] = reward.Value;
    }
}
