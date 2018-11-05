using System;
namespace Ambition
{
    public class LocationReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            AmbitionApp.SendMessage(ParisMessages.ADD_LOCATION, reward.ID);
        }
    }
}
