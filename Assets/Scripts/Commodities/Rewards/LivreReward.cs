using System;
namespace Ambition
{
    public class LivreReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            AmbitionApp.GetModel<GameModel>().Livre += reward.Value;
        }
    }
}
