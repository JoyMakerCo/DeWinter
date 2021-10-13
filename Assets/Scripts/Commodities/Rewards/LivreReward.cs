using System;
namespace Ambition
{
    public class LivreReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            int livre = AmbitionApp.Game.Livre + reward.Value;
            if (livre < 0)
            {
                AmbitionApp.Game.Livre = 0;
                AmbitionApp.SendMessage(GameMessages.OUT_OF_LIVRE, livre);
            }
            else AmbitionApp.Game.Livre = livre;
            AmbitionApp.SendMessage(GameConsts.LIVRE, AmbitionApp.Game.Livre);
            AmbitionApp.Game.Broadcast();
        }
    }
}
