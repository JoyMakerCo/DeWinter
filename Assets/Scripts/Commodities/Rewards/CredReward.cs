using System;
using Core;

namespace Ambition
{
    public class CredReward : ICommand<CommodityVO>
    {
        public  void Execute (CommodityVO reward)
        {
            int cred = reward.Value + AmbitionApp.Game.Credibility;
            if (cred < 0) AmbitionApp.Game.Credibility = 0;
            else if (cred > 100) AmbitionApp.Game.Credibility = 100;
            else AmbitionApp.Game.Credibility = cred;
            AmbitionApp.SendMessage(GameConsts.CREDIBILITY, AmbitionApp.Game.Credibility);
            AmbitionApp.Game.Broadcast();
        }
    }
}
