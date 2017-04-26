using System;
using Core;

namespace DeWinter
{
	public class VictoryCheckCmd : ICommand<GuestVO[]>
	{
		public void Execute(GuestVO[] guests)
		{
			int len = guests.Length;
			int numCharmed = Array.FindAll(guests, g=>g.LockedInState == LockedInState.Charmed).Length;
			if (numCharmed == len || Array.FindAll(guests, g=>g.LockedInState == LockedInState.PutOff).Length == len)
			{
				DeWinterApp.OpenDialog<GuestVO[]>(DialogConsts.ENCOUNTER_RESULT, guests);
			}
			if (
		//Check to see if everyone is either Charmed or Put Off 
	        int charmedAmount = 0;
	        int putOffAmount = 0;
	        foreach (GuestVO g in room.Guests)
	        {
	            if(g.lockedInState == LockedInState.Charmed)
	            {
	                charmedAmount++;
	            } else if (g.lockedInState == LockedInState.PutOff)
	            {
	                putOffAmount++;
	            }
	            //If the Conversation is Over
	            if (charmedAmount + putOffAmount == room.Guests.Length)
	            {
	                Debug.Log("Conversation Over!");
	                room.Cleared = true;
	                //Remove the Ambush Cards (If present)
	                foreach (RemarkVO r in party.playerHand)
	                {
	                    if (r.ambushRemark)
	                    {
	                        party.playerHand.Remove(r);
	                    }
	                }
	                //Rewards Distributed Here
	                Reward givenReward = room.Rewards[charmedAmount]; //Amount of Charmed Guests determines the level of Reward.
	                if (givenReward.Type() == "Introduction")
	                {
	                	ServantModel smod = DeWinterApp.GetModel<ServantModel>();
	                    foreach (Reward r in GameData.tonightsParty.wonRewardsList)
	                    {
	                        //If that Servant has already been Introduced or if the Reward of their Introduction has already been handed out then change the Reward to Gossip
							if ((r.SubType() == givenReward.SubType() && r.amount > 0) || smod.Introduced.ContainsKey(givenReward.SubType()))
	                        {
	                            givenReward = new Reward(GameData.tonightsParty, "Gossip", 1);
	                        }
	                    }
	                }
	                GameData.tonightsParty.wonRewardsList.Add(givenReward);
	                object[] objectStorage = new object[4];
	                objectStorage[0] = charmedAmount;
	                objectStorage[1] = putOffAmount;
	                objectStorage[2] = room.HostHere;
	                objectStorage[3] = givenReward;
	                screenFader.gameObject.SendMessage("WorkTheRoomReportModal", objectStorage);
	                //Close the Window
	                Destroy(gameObject);
    		}
	}
}

