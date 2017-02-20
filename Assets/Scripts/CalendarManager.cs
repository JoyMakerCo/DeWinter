using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DeWinter;

public class CalendarManager : MonoBehaviour
{
	public const int ADVANCE_RSVP_DAYS = 15;

    public SceneFadeInOut screenFader;

    private CalendarModel _calendarModel;
	private FactionModel _factionModel;

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game_Estate")
        {
			_calendarModel = DeWinterApp.GetModel<CalendarModel>();
			_factionModel = DeWinterApp.GetModel<FactionModel>();

            PrepareTonightsParty();
            PartyInvitations();
            //Missed RSVPs
        	List<Party> parties;
			if (_calendarModel.Parties.TryGetValue(_calendarModel.Yesterday, out parties))
			{
				foreach (Party party in parties)
				{
					if (party.RSVP == 0 && _factionModel[party.faction].LargestAllowableParty >= party.partySize)
					{
						AdjustBalanceVO vo = new AdjustBalanceVO("Reputation", -20);
						DeWinterApp.SendMessage<AdjustBalanceVO>(vo);

						vo.Type = party.faction;
						vo.Amount = -40;
						vo.IsRequest = true;
						DeWinterApp.SendMessage<AdjustBalanceVO>(vo);
		                screenFader.gameObject.SendMessage("CreateMissedPartyRSVPModal", party); //Party from the night before
					}
				}
			}
        }
    }

    void PrepareTonightsParty()
    {
        Debug.Log("Today is Day: " + _calendarModel.Day.ToString() + " Month: " + _calendarModel.GetMonthString());
        List<Party> parties;
        if (_calendarModel.Parties.TryGetValue(_calendarModel.Today, out parties))
        {
        	parties = parties.FindAll(x => x.RSVP == 1);
        	switch (parties.Count)
        	{
        		case 1:
        			DeWinterApp.GetModel<PartyModel>().Party = parties[0];
					Debug.Log("Tonights Party is: a " + parties[0].SizeString() + " " + parties[0].faction + " Party");
        			break;
        		case 2:
					screenFader.gameObject.SendMessage("CreateTwoPartyRSVPdPopUp", new Party[]{parties[0], parties[1]});
	    			break;    			
        	}
	    }
    }

    void PartyInvitations()
    {
    	List<Party> parties;
    	DateTime day;
    	GameModel gm = DeWinterApp.GetModel<GameModel>();
        //Party Invites (Party 1)
		for (int i = 0; i < ADVANCE_RSVP_DAYS; i++)
        {
        	day = _calendarModel.DaysFromNow(i);
        	if (_calendarModel.Parties.TryGetValue(day, out parties))
        	{
        		parties = parties.FindAll(p =>
        			p.faction != null
        			&& i <= p.invitationDistance
        			&& !p.invited
        			&& p.partySize > 0
        			&& (p.partySize <= _factionModel[p.faction].LargestAllowableParty || p.partySize <= gm.PartyInviteImportance));
        		foreach (Party party in parties)
        		{
					//Invitation Pop Up
                    screenFader.gameObject.SendMessage("CreatePartyInvitationPopUp", party);
                    //Actually Inviting the Player
                    party.InvitePlayer();
	    		}
        	}
        }
    }
}