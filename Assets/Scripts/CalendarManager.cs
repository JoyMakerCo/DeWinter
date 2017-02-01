using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CalendarManager : MonoBehaviour {

    public SceneFadeInOut screenFader;

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game_Estate")
        {
            PrepareTonightsParty();
            PartyInvitations();
            //Missed RSVPs
            if (GameData.currentDay != 0)
            {
                if (GameData.calendar.daysFromNow(-1).party1.RSVP == 0 && (GameData.factionList[GameData.calendar.daysFromNow(-1).party1.faction].largestAllowableParty() >= GameData.calendar.daysFromNow(-1).party1.partySize)) //If the Party from the night before wasn't replied to then:
                {
                    GameData.reputationCount -= 20;
                    GameData.factionList[GameData.calendar.daysFromNow(-1).party1.faction].playerReputation -= 40;
                    screenFader.gameObject.SendMessage("CreateMissedPartyRSVPModal", GameData.calendar.daysFromNow(-1).party1); //Party from the night before
                }
                if (GameData.calendar.daysFromNow(-1).party2.RSVP == 0 && (GameData.factionList[GameData.calendar.daysFromNow(-1).party2.faction].largestAllowableParty() >= GameData.calendar.daysFromNow(-1).party2.partySize)) //If the Party from the night before wasn't replied to then:
                {
                    GameData.reputationCount -= 20;
                    GameData.factionList[GameData.calendar.daysFromNow(-1).party2.faction].playerReputation -= 40;
                    screenFader.gameObject.SendMessage("CreateMissedPartyRSVPModal", GameData.calendar.daysFromNow(-1).party2); //Party from the night before
                }
            }  
        }
    }

    void PrepareTonightsParty()
    {
        Debug.Log("Today is Day: " + GameData.currentDay + " Month: " + GameData.currentMonth);
        if (GameData.calendar.today().party1.RSVP == 1 && GameData.calendar.today().party2.RSVP == 1)
        {
            screenFader.gameObject.SendMessage("CreateTwoPartyRSVPdPopUp", GameData.calendar.today());
        } else if (GameData.calendar.today().party1.RSVP != 1 && GameData.calendar.today().party2.RSVP == 1)
        {
            GameData.tonightsParty = GameData.calendar.today().party2;
        } else if (GameData.calendar.today().party1.RSVP == 1 && GameData.calendar.today().party2.RSVP != 1)
        {
            GameData.tonightsParty = GameData.calendar.today().party1;
        } else
        {
            GameData.tonightsParty = GameData.calendar.today().party1;
        }
        Debug.Log("Tonights Party is: a " + GameData.tonightsParty.partySize + " " + GameData.tonightsParty.faction + " Party");
    }

    void PartyInvitations()
    {
        //Party Invites (Party 1)
        for (int i = 0; i < 15; i++)
        {
            Party party = GameData.calendar.daysFromNow(i).party1;
            if (party.faction != null)
            {
                //Is the Party within Invite Range? && Has the Player been Invited Already? && Is it a real Party at all?
                if (i <= party.invitationDistance && party.invited == false && party.partySize > 0)
                {
                    //Is the Player of the Right Faction Benefit Level? 
                    if (party.partySize <= GameData.factionList[party.faction].largestAllowableParty())
                    {
                        //Invitation Pop Up
                        screenFader.gameObject.SendMessage("CreatePartyInvitationPopUp", party);
                        //Actually Inviting the Player
                        party.InvitePlayer();
                    }
                    //Or do they have just such a Reputation that they get invited anyway?
                    else if (party.partySize <= GameData.reputationLevels[GameData.playerReputationLevel].PartyInviteImportance())
                    {
                        //Invitation Pop Up
                        screenFader.gameObject.SendMessage("CreatePartyInvitationPopUp", party);
                        //Actually Inviting the Player
                        party.InvitePlayer();
                    }
                }        
            }
        }
        //Party Invites (Party 2)
        for (int i = 0; i < 15; i++)
        {
            if (GameData.calendar.daysFromNow(i).party2 != null)
            {
                Party party = GameData.calendar.daysFromNow(i).party2;
                if (party.faction != null)
                {
                    //Is the Party within Invite Range? && Has the Player been Invited Already? && Is it a real Party at all?
                    if (i <= party.invitationDistance && party.invited == false && party.partySize > 0)
                    {
                        //Is the Player of the Right Faction Benefit Level? 
                        if (party.partySize <= GameData.factionList[party.faction].largestAllowableParty())
                        {
                            //Invitation Pop Up
                            screenFader.gameObject.SendMessage("CreatePartyInvitationPopUp", party);
                            //Actually Inviting the Player
                            party.InvitePlayer();
                        }
                        //Or do they have just such a Reputation that they get invited anyway?
                        else if (party.partySize <= GameData.reputationLevels[GameData.playerReputationLevel].PartyInviteImportance())
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
    }
}
