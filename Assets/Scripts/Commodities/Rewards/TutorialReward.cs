using System;
using Core;
namespace Ambition
{
    public class TutorialReward : ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            bool addRewardID = true;
            switch (reward.ID)
            {
                case TutorialConsts.TUTORIAL_MARKET:
                    AmbitionApp.UFlow.Register<MarketTutorialController>(reward.ID);
                    break;
                case TutorialConsts.TUTORIAL_CALENDAR:
                    AmbitionApp.UFlow.Register<CalendarTutorialController>(reward.ID);
                    break;
                case TutorialConsts.TUTORIAL_GOSSIP:
                    AmbitionApp.UFlow.Register<GossipTutorialController>(reward.ID);
                    break;
                case TutorialConsts.TUTORIAL_INCIDENT:
                    AmbitionApp.UFlow.Register<IncidentTutorialController>(reward.ID);
                    break;
                case TutorialConsts.TUTORIAL_PARTY:
                    AmbitionApp.UFlow.Register<PartyTutorialController>(reward.ID);
                    break;
                case TutorialConsts.TUTORIAL_WARDROBE:
                    AmbitionApp.UFlow.Register<WardrobeTutorialController>(reward.ID);
                    break;
                default: addRewardID = false; break;
            }
            if (addRewardID && !AmbitionApp.Game.Tutorials.Contains(reward.ID))
            { 
                AmbitionApp.Game.Tutorials.Add(reward.ID);
            }
        }
    }
}
