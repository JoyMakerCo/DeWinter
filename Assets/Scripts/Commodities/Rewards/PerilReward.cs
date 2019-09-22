using System;
using Core;
namespace Ambition
{
    public class PerilReward : ICommand<CommodityVO>
    {
        public  void Execute (CommodityVO peril)
        {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            if (model.Peril.Value + peril.Value > 0) model.Peril.Value += peril.Value;
            else model.Peril.Value = 0;
        }
    }
}
