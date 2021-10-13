using System;
namespace Ambition
{
    public class ApplyExhaustionPenaltyCmd : Core.ICommand
    {
        public void Execute()
        {
            CommodityVO penalty = new CommodityVO(CommodityType.Credibility, AmbitionApp.Game.ExhaustionPenalty);
            AmbitionApp.SendMessage(penalty);
        }
    }
}
