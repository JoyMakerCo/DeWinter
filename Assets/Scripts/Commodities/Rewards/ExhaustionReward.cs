using System;
namespace Ambition
{
    public class ExhaustionReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            AmbitionApp.Game.Exhaustion += reward.Value;
        }
    }
}
