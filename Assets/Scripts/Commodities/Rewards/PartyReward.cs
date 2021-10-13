using System;
using System.Collections.Generic;

namespace Ambition
{
    public class PartyReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (!string.IsNullOrEmpty(reward.ID))
            {
                PartyVO party = new PartyVO(reward.ID);
                if (reward.Value >= 0) party.Day = AmbitionApp.Calendar.Day + reward.Value;
                AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
            }
        }
    }
}
