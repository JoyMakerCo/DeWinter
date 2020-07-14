using System;
using System.Collections.Generic;

namespace Ambition
{
    public class PartyReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (reward.ID != null)
            {
                PartyVO party;
                PartyModel model = AmbitionApp.GetModel<PartyModel>();
                if (!model.Parties.TryGetValue(reward.ID, out party))
                {
                    AmbitionApp.SendMessage(PartyMessages.LOAD_PARTY, reward.ID);
                    model.Parties.TryGetValue(reward.ID, out party);
                }
                if (party != null)
                {
                    switch (party.RSVP)
                    {
                        case RSVP.Accepted:
                        case RSVP.Required:
                            AmbitionApp.SendMessage(PartyMessages.ACCEPT_INVITATION, party);
                            break;
                        case RSVP.Declined:
                            AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, party);
                            break;
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Warning: PartyReward.cs: No party with ID \"" + reward.ID + "\" exists!");
                }
                UnityEngine.Resources.UnloadUnusedAssets();
            }
            else
                UnityEngine.Debug.Log("Warning: PartyReward.cs: No party ID specified!");

        }
    }
}
