using System;
namespace Ambition
{
    public class ServantReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            ServantModel model = AmbitionApp.GetModel<ServantModel>();
            switch(reward.Value)
            {
                case -1:
                    AmbitionApp.SendMessage(ServantMessages.FIRE_SERVANT, reward.ID);
                    model.Servants.RemoveAll(s => s.ID == reward.ID);
                    model.Broadcast();
                    break;
                case 0:
                    ServantVO servant = model.GetServant(reward.ID);
                    if (servant?.IsHired ?? false)
                    {
                        AmbitionApp.SendMessage(ServantMessages.FIRE_SERVANT, reward.ID);
                    }
                    else AmbitionApp.SendMessage(ServantMessages.INTRODUCE_SERVANT, reward.ID);
                    break;
                default:
                    AmbitionApp.SendMessage(ServantMessages.HIRE_SERVANT, reward.ID);
                    break;
            }
        }
    }
}
