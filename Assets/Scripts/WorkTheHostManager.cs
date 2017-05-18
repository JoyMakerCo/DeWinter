using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Ambition;

public class WorkTheHostManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //Set Up The Host
        hostNameText = hostVisual.transform.Find("Name").GetComponent<Text>();
        hostNameText.text = party.host.Name;
        hostImage = hostVisual.transform.Find("HostImage").GetComponent<Image>();
        hostImage.sprite = GenerateHostImage();
        hostInterestBar = hostVisual.transform.Find("InterestBar").GetComponent<Scrollbar>();
        hostInterestBarBackground = hostVisual.transform.Find("InterestBar").GetComponent<Image>();
        hostOpinionBar = hostVisual.transform.Find("OpinionBar").GetComponent<Scrollbar>();
        hostDispositionIcon = hostVisual.transform.Find("DispositionIcon").GetComponent<Image>();
        hostDispositionIcon.color = HostDispositionImageColor();

        hostDispositionSwitchTimer = party.host.dispositionTimerSwitchMax;
        hostRemarkTimer = party.host.nextHostRemarkTimer;

        //Is the Player using the Fan Accessory? If so then increase the Host Remark Time by 1.5x!
		if(GameData.partyAccessory != null && GameData.partyAccessory.Type == "Fan")
		{
            party.host.currentInterestTimer = party.host.currentInterestTimer * 1.5f;
            party.host.maxInterestTimer = party.host.currentInterestTimer * 1.5f;
        }

        //Turn Timer
        if(GameData.playerReputationLevel >= 5)
        {
            maxTurnTimer = (float)(maxTurnTimer * 1.1);
            currentTurnTimer = maxTurnTimer;
        }
        turnTimerBar.value = currentTurnTimer / maxTurnTimer;
        if (GameData.playerReputationLevel >= 2)
        {
            reparteeIndicatorImage.color = Color.green;
        }
        else
        {
            reparteeIndicatorImage.color = Color.clear;
        }

        //Start/Intro Timer
        StartCoroutine(ConversationStartTimerWait());
    }

    // Update is called once per frame
    void Update()
    {
        playerConfidenceText.text = "Confidence: " + party.currentPlayerConfidence + "/" + party.maxPlayerConfidence;
        playerConfidenceBar.value = (float)party.currentPlayerConfidence / party.maxPlayerConfidence;
        if (party.currentPlayerConfidence <= 25)
        {
            playerConfidenceText.color = Color.red;
        }
        else
        {
            playerConfidenceText.color = Color.white;
        }
        playerIntoxicationText.text = "Intoxication: " + party.currentPlayerIntoxication + "/" + party.maxPlayerIntoxication;
        playerDrinkAmountText.text = "Booze Glass: " + party.currentPlayerDrinkAmount + "/" + party.maxPlayerDrinkAmount;
        playerDrinkAmountText.text = "Booze Glass: " + party.currentPlayerDrinkAmount + "/" + party.maxPlayerDrinkAmount;
        if (party.currentPlayerDrinkAmount > 0)
        {
            drinkBoozeButtonImage.color = Color.white;
        }
        else
        {
            drinkBoozeButtonImage.color = Color.gray;
        }

        //Host--------
        hostInterestText.text = InterestState();
        hostInterestBar.value = InterestTimer();
        hostOpinionBar.value = (float)party.host.currentOpinion / party.host.maxOpinion;
        hostDispositionIcon.color = HostDispositionImageColor();

        //Interest Timers are only displayed when the Host isn't locked in
        InterestTimersDisplayCheck();

        //Interest Timers and Boredom Damage
        //Turn Timer
        if (conversationStarted && turnTimerActive && !hostRemarkActive)
        {
            currentTurnTimer -= Time.deltaTime;
            turnTimerBar.value = currentTurnTimer / maxTurnTimer;
            if (currentTurnTimer <= 0)
            {
                EndTurn();
            }
        }
        else if (conversationStarted && !turnTimerActive && !hostRemarkActive)
        {
            StartCoroutine(NextTurnTimerWait());
        }

        //Visualizing the Remarks
        VisualizeRemarks();

        //Check to see if the Host has anything to say
        HostRemarkCheck();

        //Advance the Timer on when the Host will switch their Disposition
        hostDispositionSwitchTimer -= Time.deltaTime;

        //If the Host has an Active Remark Against the Player
        if (hostRemarkActive && hostRemarkStarted)
        {
            party.host.hostRemarkCompletionTimerCurrent -= Time.deltaTime;
            hostRemarkCountdownBar.value = party.host.hostRemarkCompletionTimerCurrent / party.host.hostRemarkCompletionTimerMax;
            //Did the Player not clear it in time?
            if (party.host.hostRemarkCompletionTimerCurrent < 0)
            {
                party.host.currentOpinion -= 20;
                party.currentPlayerConfidence -= 25;
                //Kill the Window
                Destroy(hostRemarkWindow);
                party.host.hostRemarkCompletionTimerCurrent = party.host.hostRemarkCompletionTimerMax;
                hostRemarkActive = false;
                Debug.Log("Host Remark Failed! :-(");
            }
            //Did the Player clear it in time?
            if (hostRemarkSlotComplete >= hostRemarkSlotList.Count)
            {
                //Kill the Window
                Destroy(hostRemarkWindow);
                party.host.hostRemarkCompletionTimerCurrent = party.host.hostRemarkCompletionTimerMax;
                hostRemarkActive = false;
                Debug.Log("Host Remark Defeated! :-D");
            }
        }

        //Listening for Hotkeys----------------
        ListenForHotkeys();

        //Victory and Defeat Checks-------------
        VictoryCheck();
        ConfidenceCheck();
    }

    void HostTargeted()
    {
        //Do they like the Tone?
        if (party.playerHand[targetingRemark].tone == party.host.disposition.like) //They like the tone
        {
            ChangeHostOpinion(Random.Range(25, 36));
            AddRemarkToHand(); //Add a new Remark for Tone success           
            party.currentPlayerConfidence = Mathf.Clamp(party.currentPlayerConfidence + 5, 5, party.maxPlayerConfidence); //Confidence Reward
            party.host.dispositionRevealed = true; // Reveal their Disposition to the Player (if concealed)
        }
        else if (party.playerHand[targetingRemark].tone == party.host.disposition.dislike) //They dislike the tone
        {
            ChangeHostOpinion(Random.Range(-17, -11));
            party.currentPlayerConfidence = Mathf.Clamp(party.currentPlayerConfidence - 10, 0, party.maxPlayerConfidence); //Confidence Penalty
        }
        else //Neutral Tone
        {
            ChangeHostOpinion(Random.Range(12, 18));
        }
        // Refill Interest of the Selected
        party.host.currentInterestTimer = party.host.maxInterestTimer + 1; //End Turn already Knocks one off so this compensates
        party.host.interestTimerWaiting = true;
        StartCoroutine(party.host.TimerWait());
        //Check to see if the Disposition Should Switch
        HostDispositionSwitchCheck();
    }

    public void HostHighlighting()
    {
        if (targetingMode)
        {
            hostImage.color = ReactionColor();
        }
    }

    public void HostUnhighlighting()
    {
        hostImage.color = Color.white;
    }

    public void HostRemarkSelected(int slotNumber)
    {
        if (targetingMode) //If a remark has been selected
        {
            //Targeting Profiles Get Taken into Account Here
            switch (party.playerHand[targetingRemark].targetingProfileInt)
            {

                case 1:
                    //No flip or failsafe version necessary, it's just one target
                    HostRemarkTargeted(slotNumber);
                    break;
                case 11:
                    //Flip versions and failsafes
                    if (!targetingFlipped)
                    {
                        HostRemarkTargeted(slotNumber);
                        HostRemarkTargeted(slotNumber + 1);
                    }
                    else
                    {
                        HostRemarkTargeted(slotNumber);
                        HostRemarkTargeted(slotNumber - 1);
                    }
                    break;
                case 101:
                    //Flip Versions and Failsafes
                    if (!targetingFlipped)
                    {
                        HostRemarkTargeted(slotNumber);
                        HostRemarkTargeted(slotNumber + 2);
                    }
                    else
                    {
                        HostRemarkTargeted(slotNumber);
                        HostRemarkTargeted(slotNumber - 2);
                    }
                    break;
                case 1011:
                    //Flip version but no failsafes, as it covers all 4 Guests
                    if (!targetingFlipped)
                    {
                        HostRemarkTargeted(slotNumber);
                        HostRemarkTargeted(slotNumber + 2);
                        HostRemarkTargeted(slotNumber + 3);
                    }
                    else
                    {
                        HostRemarkTargeted(slotNumber);
                        HostRemarkTargeted(slotNumber - 2);
                        HostRemarkTargeted(slotNumber - 3);
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("No remark selected :(");
        }
        //Deselect any remarks
        party.playerHand.RemoveAt(targetingRemark);
        targetingRemark = -1;
        targetingMode = false;
        if (party.playerHand.Count == 0)
        {
            SpendConfidenceGetRemark();
        }
    }

    public Color ReactionColor()
    {
        if (!party.host.dispositionRevealed && party.currentPlayerIntoxication >= 50)
        {
            return Color.gray;
        }
        if (party.playerHand[targetingRemark].tone == party.host.disposition.like) //They like the tone
        {
            return Color.green;
        }
        else if (party.playerHand[targetingRemark].tone == party.host.disposition.dislike) //They dislike the tone
        {
            return Color.red;
        }
        else //Neutral Tone
        {
            return Color.gray;
        }
    }

    public Color HostRemarkSlotReactionColor(int slotNumber)
    {
        if (party.playerHand[targetingRemark].tone == hostRemarkSlotList[slotNumber].disposition.like) //They like the tone
        {
            return Color.green;
        }
        else if (party.playerHand[targetingRemark].tone == hostRemarkSlotList[slotNumber].disposition.dislike) //They dislike the tone
        {
            return Color.red;
        }
        else //Neutral Tone
        {
            return Color.gray;
        }
    }

    Color HostDispositionImageColor()
    {
        if (party.currentPlayerIntoxication >= 50 && party.host.dispositionRevealed == false)
        {
            return Color.gray;
        }
        else
        {
            return GameData.dispositionList[party.host.dispositionInt].color;
        }
    }

    void EndTurn()
    {
        //Reset the Turn Timer
        currentTurnTimer = maxTurnTimer;
        turnTimerBar.value = currentTurnTimer / maxTurnTimer;

        //Increment the Host's Interest Timer, issue Boredom Damage
        party.host.currentInterestTimer = Mathf.Clamp(party.host.currentInterestTimer - 1, 0, party.host.maxInterestTimer);
        if (party.host.currentInterestTimer <= 0 && party.host.notableLockedInState == GuestState.Ambivalent)
        { 
            party.host.currentOpinion = Mathf.Clamp(party.host.currentOpinion - 10, 0, party.host.maxOpinion);
            if (party.host.currentOpinion <= 0)
            {
                party.host.notableLockedInState = GuestState.PutOff;
            }
        }

        //Pause the Next Turn Timer
        turnTimerActive = false;
    }


    public void SpendConfidenceGetRemark()
    {
        int confidenceCost = 10;
        if (party.currentPlayerConfidence >= confidenceCost && party.playerHand.Count < 5)
        {
            party.currentPlayerConfidence -= confidenceCost;
            AddRemarkToHand();
        }
    }


    public Sprite VisualizeTargetingProfile(int remarkInt)
    {
        switch (party.playerHand[remarkInt].targetingProfileInt)
        {
            case 1:
                return targetingProfile1;
            case 11:
                return targetingProfile11;
            case 101:
                return targetingProfile101;
            case 1011:
                return targetingProfile1011;
            default:
                return null;
        }
    }


    void AddRemarkToHand()
    {
        if (party.playerHand.Count < 6) // This is one larger than it should be because Remarks are deducted after they're added
        {
            RemarkVO remark = new RemarkVO(party.lastTone, room.Guests.Length);
            party.lastTone = remark.tone;
            party.playerHand.Add(remark);
        }
    }

    void RefillPlayerHand()
    {
        int numberOfCardsForRefill = 5 - party.playerHand.Count;
        for (int i = 0; i < numberOfCardsForRefill; i++)
        {
            RemarkVO remark = new RemarkVO(party.lastTone, room.Guests.Length);
            party.lastTone = remark.tone;
            party.playerHand.Add(remark);
        }
    }

    public void TargettingFlip()
    {
        targetingFlipped = !targetingFlipped;
    }

    public void ExchangeRemark()
    {
        if (targetingMode)
        {
            party.playerHand.RemoveAt(targetingRemark);
            AddRemarkToHand();
            EndTurn();
        }
    }

    float InterestTimer()
    {
        return party.host.currentInterestTimer / party.host.maxInterestTimer;
    }

    void VictoryCheck()
    {
    	string dialogType=null;
    	Reward reward = null;
    	Dictionary<string, string> subs = new Dictionary<string, string>();
		switch (party.host.notableLockedInState)
		{
			case GuestState.Charmed:
				dialogType = DialogConsts.CHARMED_HOST_DIALOG;
				reward = room.Rewards[5];
				break;
			case GuestState.PutOff:
				dialogType = DialogConsts.FAILED_HOST_DIALOG;
				reward = room.Rewards[5];
				break;
			default:
				break;
		}
		if (dialogType != null && reward != null)
		{
			room.Cleared = true;
			GameData.tonightsParty.wonRewardsList.Add(reward);
			subs.Add("$REWARD",reward.Name());
			AmbitionApp.OpenMessageDialog(dialogType, subs);

			if (hostRemarkWindow != null)
            {
                Destroy(hostRemarkWindow);
            }
            Destroy(gameObject);
		}
    }

    void ConfidenceCheck()
    {
        //Check to see if the Player has run out of Confidence
        if (party.currentPlayerConfidence <= 0)
        {
            //The Player loses a turn
			AmbitionApp.AdjustValue<int>(PartyConstants.TURNSLEFT, -1);
            //The Player has their Confidence Reset
            party.currentPlayerConfidence = party.startingPlayerConfidence / 2;
            //The Player is relocated to the Entrance
            roomManager.MovePlayerToEntrance();
            //The Player's Reputation is Punished
            int reputationLoss = 25;
            int factionReputationLoss = 50;
            GameData.reputationCount -= reputationLoss;
			AmbitionApp.AdjustValue<>(new AdjustValueVO(party.faction, -factionReputationLoss));
            //Explanation Screen Pop Up goes here
            Dictionary<string, string> subs = new Dictionary<string, string>(){
				{"$FACTIONREPUTATION",factionReputationLoss.ToString()},
				{"$FACTION",party.faction},
				{"$REPUTATION",reputationLoss.ToString()}};
            AmbitionApp.OpenMessageDialog(DialogConsts.OUT_OF_CONFIDENCE_DIALOG, subs);

            //The Player is pulled from the Work the Room session
            Destroy(gameObject);
        }
    }
    void InterestTimersDisplayCheck()
    {
        if (party.host.notableLockedInState != GuestState.Ambivalent)
        {
            hostInterestBar.image.color = Color.clear;
            hostInterestBarBackground.color = Color.clear;
        }
        else
        {
            hostInterestBar.image.color = Color.white;
            hostInterestBarBackground.color = Color.white;
        }
    }

    public IEnumerator ConversationStartTimerWait()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(readyGoPanel);
        Destroy(readyGoText);
        conversationStarted = true;
    }

    public IEnumerator NextTurnTimerWait()
    {
        yield return new WaitForSeconds(0.75f);
        turnTimerActive = true;
    }

    void HostDispositionSwitchCheck()
    {
        if (hostDispositionSwitchTimer <= 0)
        {
            //Actually Changes the disposition
            party.host.ChangeDisposition();
            hostDispositionSwitchTimer = party.host.dispositionTimerSwitchMax;
            hostDispositionIcon.color = GameData.dispositionList[party.host.dispositionInt].color;
        }
    }

    void HostRemarkCheck()
    {
        if (!hostRemarkActive)
        {
            //Timer Stuff
            hostRemarkTimer -= Time.deltaTime;
            //If the Timer is sufficiently low then it's Fire Back Remark Time!
            if (hostRemarkTimer <= 0)
            {
                //Creates the Host Remark Modal
                object[] objectStorage = new object[3];
                objectStorage[0] = party.host;
                objectStorage[1] = this;
                objectStorage[2] = Random.Range(1, 6);
                screenFader.gameObject.SendMessage("CreateHostRemarkModal", objectStorage);
                Text introText = hostRemarkWindow.transform.Find("IntroTextPanel").Find("Text").GetComponent<Text>();
				string[] hostRemarkIntroList = AmbitionApp.GetModel<PartyModel>().HostIntros;
                introText.text = hostRemarkIntroList[Random.Range(0, hostRemarkIntroList.Length)];
                //So there'll be a wait before the next Host Remark
                hostRemarkTimer = party.host.nextHostRemarkTimer;
                //Kills the Wait Screen
                StartCoroutine(HostRemarkTimerWait());
            }
        }      
    }

    public IEnumerator HostRemarkTimerWait()
    {
        Debug.Log("'But I have a Question' Timer Started");
        yield return new WaitForSeconds(1.0f);
        GameObject introTextPanel = hostRemarkWindow.transform.Find("IntroTextPanel").gameObject;
        Destroy(introTextPanel);
        hostRemarkStarted = true;
    }

    void ListenForHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DrinkForConfidence();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            TargettingFlip();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpendConfidenceGetRemark();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExchangeRemark();
        }
    }

    void VisualizeRemarks()
    {
        for (int i = 0; i < remarkSlotList.Count; i++)
        {
            if (party.playerHand.ElementAtOrDefault(i) != null)
            {
                if (!party.playerHand[i].ambushRemark)
                {

                    remarkSlotList[i].targetingProfileImage.color = Color.white;
                    remarkSlotList[i].targetingProfileImage.sprite = VisualizeTargetingProfile(i);
                    remarkSlotList[i].dispositionIcon.color = GameData.dispositionList[party.playerHand[i].toneInt].color;
                    if (targetingFlipped)
                    {
                        remarkSlotList[i].targetingProfileImage.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else
                    {
                        remarkSlotList[i].targetingProfileImage.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
                else
                {
                    remarkSlotList[i].targetingProfileImage.color = Color.black;
                    remarkSlotList[i].dispositionIcon.color = Color.black;
                }
            }
            else
            {
                remarkSlotList[i].targetingProfileImage.color = Color.clear;
                remarkSlotList[i].dispositionIcon.color = Color.clear;
            }
        }
    }    
}