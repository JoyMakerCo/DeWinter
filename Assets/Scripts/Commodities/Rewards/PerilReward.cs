using Core;
namespace Ambition
{
    public class PerilReward : ICommand<CommodityVO>
    {
        public  void Execute (CommodityVO reward)
        {
            int peril = AmbitionApp.Game.Peril + reward.Value;
            if (peril < 0) AmbitionApp.Game.Peril = 0;
            else if (peril > 100) AmbitionApp.Game.Peril = 100;
            else AmbitionApp.Game.Peril = peril;
            AmbitionApp.SendMessage(GameConsts.PERIL, AmbitionApp.Game.Peril);
            AmbitionApp.Game.Broadcast();
        }
    }
}
