using System;
namespace Ambition
{
    public class MessageReward : Core.ICommand<CommodityVO>
    {
        // This is so redundant it hurts
        public void Execute(CommodityVO reward)
        {
            AmbitionApp.SendMessage(reward.ID);
        }
    }
}
