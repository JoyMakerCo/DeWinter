using System;
namespace Ambition
{
    public class ServantReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            ServantModel servants = AmbitionApp.GetModel<ServantModel>();
            ServantVO servant = null;
            if (!servants.Servants.ContainsKey(reward.ID) && servants.Applicants.ContainsKey(reward.ID))
            {
                servant = Util.RNG.TakeRandom(servants.Applicants[reward.ID].ToArray());
                servants.Hire(servant);
            }
            if (servant == null)
            {
                reward.Type = CommodityType.Gossip;
                AmbitionApp.Reward(reward);
            }
        }
    }
}
