using System;
using Core;

namespace Ambition
{
	public class EndConversationCmd : ICommand
	{
		public void Execute()
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            int numCharmed = 0; //Array.FindAll(model.Guests, g=>g.State == GuestState.Charmed).Length;
            AmbitionApp.UnregisterModel<ConversationModel>();
/*
            RoomVO room = AmbitionApp.GetModel<MapModel>();
            room.Cleared = true;

            if (room.Rewards != null && room.Rewards.Length > 0)
            {
                model.Party.Rewards.AddRange(room.Rewards);
                AmbitionApp.SendMessage(room.Rewards);
            }
            else
            {
                CommodityVO reward = GenerateRandomReward(numCharmed, model.Party.Faction.ToString());
                model.Party.Rewards.Add(reward);
                AmbitionApp.SendMessage(reward);
            }
            int numRemarks = (int)(AmbitionApp.GetModel<PartyModel>().MaxDeckSize * .1f);
            AmbitionApp.SendMessage(PartyMessages.RESHUFFLE_REMARKS, numRemarks);
*/          
		}

		private CommodityVO GenerateRandomReward(int numCharmed, string faction)
    	{
    		int factor = numCharmed < 5 ? numCharmed : 6;
			switch ( Util.RNG.Generate(5))
			{
				case 0:
				case 1:
					return new CommodityVO(CommodityType.Reputation, 5*factor);
				case 2:
				case 3:
                    return new CommodityVO(CommodityType.Reputation, faction, 10*factor);
			}
			return (numCharmed < 5)
                ? new CommodityVO(CommodityType.Reputation, faction, 1)
				: new CommodityVO(CommodityType.Servant, ServantConsts.SEAMSTRESS, 1);
		}
	}
}
