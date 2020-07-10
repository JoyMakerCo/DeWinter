using System;
namespace Ambition
{
    public class IncidentReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            GameModel game = AmbitionApp.GetModel<GameModel>();
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            model.LoadIncident(reward.ID, IncidentType.Party);
            model.Schedule(reward.ID, game.Day + reward.Value - 1);
        }
    }
}
