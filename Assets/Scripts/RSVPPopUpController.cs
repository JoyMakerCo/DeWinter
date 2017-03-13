using UnityEngine;
using System.Collections;
using DeWinter;

public class RSVPPopUpController : MonoBehaviour {

    public Party party; //The Party being agreed to or cancelled
    public bool today; //If the cancellation is happening today then this is true and the penalties are more severe

    public void RSVPAction(int decision)
    {
        //0 means no RSVP yet, 1 means Attending and -1 means Decline
        party.RSVP = decision;
    }

    public void CancellationAction()
    {
        party.RSVP = -1;
        if (today)
        {
        	DeWinterApp.SendMessage<AdjustValueVO>(new AdjustValueVO(party.faction, -40));
            GameData.reputationCount -= 20;
        } else
        {
			DeWinterApp.SendMessage<AdjustValueVO>(new AdjustValueVO(party.faction, -20));
            GameData.reputationCount -= 10;
        }
    }
}
