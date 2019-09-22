using System;
using Core;

namespace Ambition
{
    public class CredReward : ICommand<CommodityVO>
    {
        public  void Execute (CommodityVO cred)
        {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            if (cred.Value + model.Credibility.Value > 0) model.Credibility.Value += cred.Value;
            else model.Credibility.Value = 0;
        }
    }
}
