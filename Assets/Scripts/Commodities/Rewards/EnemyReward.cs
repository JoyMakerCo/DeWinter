using System;
namespace Ambition
{
    public class EnemyReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            AmbitionApp.SendMessage<string>(GameMessages.CREATE_ENEMY, reward.ID);
        }
    }
}
