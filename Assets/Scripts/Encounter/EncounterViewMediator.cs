using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace DeWinter
{
	public class EncounterViewMediator : MonoBehaviour
	{
// Everything left-aligned under this comment should be scripted separately
Text title;
public Text playerDrinkAmountText;
public GameObject playerVisual;
Text playerNameText;
Text playerIntoxicationText;
Image playerImage;

		public KeyValuePair<BitArray, Sprite>[] RemarkSprites;
		public KeyValuePair<string, Sprite>[] Dispositions;
		public GuestSprite[] GuestSprites;
		public GuestViewMediator[] GuestViews;
		public RemarkViewMediator[] RemarkViews;
		public Image drinkBoozeButtonImage;

		private PartyModel _model;
		private RoomVO _room;
		private Remark _selectedRemark=null;

		public Image reparteeIndicatorImage;

	    public Image readyGoPanel;
	    public Text readyGoText;

		void Awake()
		{
// TODO: Add these to the objects themselves
title = this.transform.Find("TitleText").GetComponent<Text>();
playerNameText = playerVisual.transform.Find("Name").GetComponent<Text>();
playerNameText.text = "Yvette";
drinkBoozeButtonImage = playerVisual.transform.Find("DrinkBoozeButton").GetComponent<Image>();
playerIntoxicationText = playerVisual.transform.Find("DrinkBoozeButton").Find("IntoxicationCounter").GetComponent<Text>();

			_model = DeWinterApp.GetModel<PartyModel>();
			DeWinterApp.Subscribe<RoomVO>(HandleRoom);
			DeWinterApp.Subscribe<List<Remark>>(HandleHand);
			DeWinterApp.Subscribe<float>(PartyMessages.START_TIMERS, HandleTimers);
			DeWinterApp.Subscribe<int>(GameConsts.INTOXICATION, HandleBooze);
		}

		void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<RoomVO>(HandleRoom);
			DeWinterApp.Unsubscribe<List<Remark>>(HandleHand);
			DeWinterApp.Unsubscribe<float>(PartyMessages.START_TIMERS, HandleTimers);
			DeWinterApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleBooze);
	    }

		private void HandleRoom(RoomVO room)
		{
			_room = room;
			title.text = _room.Name;
		}

		private void HandleHand(List<Remark> Hand)
		{
			bool isRemark;
			for (int i = RemarkViews.Length-1; i>=0; i--)
			{
				isRemark = (i < Hand.Count);
				RemarkViews[i].enabled = isRemark;
				if (isRemark) RemarkViews[i].Remark = Hand[i];
			}
		}

	    bool fascinatorEffect; //The Fascinator Accessory lets the first negative comment go ignored during each Conversation

	    // Use this for initialization
	    void Start()
	    {

playerIntoxicationText.text = "Intoxication: " + _model.Party.currentPlayerIntoxication + "/" + _model.Party.maxPlayerIntoxication;

			reparteeIndicatorImage.enabled = (GameData.playerReputationLevel >= 2);

	        //Ready Go Text
			string[] conversationIntroList = DeWinterApp.GetModel<PartyModel>().ConversationIntros;
	        readyGoText.text = conversationIntroList[Random.Range(0, conversationIntroList.Length)];

	        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
	        // TODO: Passive buff system
			fascinatorEffect = (GameData.partyAccessory != null && GameData.partyAccessory.Type == "Fascinator");

	        //Tutorial Pop-Up? Only used in the tutorial Room
	        // This can be made less hacky by registering the dialog in a command triggered by entering a room
	        if (_model.Party.tutorial)
	        {
				DeWinterApp.OpenDialog("WorkTheRoomTutorialPopUpModal");
	        } else
	        {
	        	DeWinterApp.SendMessage<float>(PartyMessages.START_TIMERS, _model.TurnTimer);
	        }
	    }

	    // Update is called once per frame
	    void Update()
	    {
			if(Input.GetKeyDown(KeyCode.D))
	        {
	            DrinkForConfidence();
	        }
	        else if (Input.GetKeyDown(KeyCode.F))
	        {
	            TargettingFlip();
	        }
			else if (Input.GetKeyDown(KeyCode.C))
	        {
	            SpendConfidenceGetRemark();
	        }
			else if (Input.GetKeyDown(KeyCode.E))
	        {
	            ExchangeRemark();
	        }
        }


		private void HandleBooze(int tox)
		{
			playerIntoxicationText.text = "Intoxication: " + tox.ToString() + "/" + _model.Party.maxPlayerIntoxication;
			playerDrinkAmountText.text = "Booze Glass: " + tox.ToString() + "/" + _model.Party.maxPlayerDrinkAmount;
			drinkBoozeButtonImage.color (tox > 0) ? Color.white : Color.gray;
		}
	        
	        //Visualizing the Remarks--------------
//	        VisualizeRemarks();
	        
	        //Turn Timer
//	        if(conversationStarted && turnTimerActive)
//	        {
//	            room.TurnTimer -= Time.deltaTime;
//	            turnTimerBar.value = room.TurnTimer / maxTurnTimer;
//	            if (room.TurnTimer <= 0)
//	            {
//	                EndTurn();
//	            }
//	        } else if (conversationStarted && !turnTimerActive)
//	        {           
//	            StartCoroutine(NextTurnTimerWait());
//	        }        
//
//	        //Victory and Defeat Checks------------ 
//	        VictoryCheck();
//	        ConfidenceCheck();

		private void HandleTimers(float seconds)
		{
			StartCoroutine(TimerCoroutine(seconds));
		}

		IEnumerator TimerCoroutine(float seconds)
		{
			yield return WaitForSeconds(seconds);
			EndTurn();
		}

	    public void StartTargeting(int selectedRemark)
	    {
	        if (conversationStarted)
	        {
	            if (party.playerHand[selectedRemark] != null && !party.playerHand[selectedRemark].ambushRemark)
	            {
	                targetingMode = true;
	                targetingRemark = selectedRemark;
	                //Debug.Log("Remark Tone: " + GameData.dispositionList[party.playerHand[selectedRemark].toneInt].like + " " + party.playerHand[selectedRemark].toneInt);
	                //Debug.Log("Remark Targeting Profile: " + party.playerHand[selectedRemark].targetingProfileInt);
	            }
	            else
	            {
	                Debug.Log("No Remark to Select");
	            }
	        }
	    }

	    public void GuestSelected(int guestNumber)
	    {
	        if (targetingMode) //If a remark has been selected
	        {
	            //Targeting Profiles Get Taken into Account Here
	            switch (party.playerHand[targetingRemark].targetingProfileInt)
	            {          
	                case 1:
	                    //No flip or failsafe version necessary, it's just one target
	                    GuestTargeted(guestNumber);
	                    break;
	                case 11:
	                    //Flip versions and failsafes
	                    if (!targetingFlipped)
	                    {
	                        GuestTargeted(guestNumber);
	                        GuestTargeted(guestNumber+1);
	                    }
	                    else
	                    {
	                        GuestTargeted(guestNumber);
	                        GuestTargeted(guestNumber - 1);
	                    }
	                    break;
	                case 101:
	                    //Flip Versions and Failsafes
	                    if (!targetingFlipped)
	                    {
	                        GuestTargeted(guestNumber);
	                        GuestTargeted(guestNumber + 2);
	                    }
	                    else
	                    {
	                        GuestTargeted(guestNumber);
	                        GuestTargeted(guestNumber - 2);
	                    }
	                    break;
	                case 1011:
	                    //Flip version but no failsafes, as it covers all 4 Guests
	                    if (!targetingFlipped)
	                    {
	                        GuestTargeted(guestNumber);
	                        GuestTargeted(guestNumber + 2);
	                        GuestTargeted(guestNumber + 3);
	                    } else
	                    {
	                        GuestTargeted(guestNumber);
	                        GuestTargeted(guestNumber - 2);
	                        GuestTargeted(guestNumber - 3);
	                    }
	                    break;
	            }
	            //Deselect any remarks
	            party.playerHand.RemoveAt(targetingRemark);
	            targetingRemark = -1;
	            targetingMode = false;
	            if (party.playerHand.Count == 0)
	            {
	                SpendConfidenceGetRemark();
	            }
	            EndTurn();
	        } else
	        {
	            Debug.Log("No remark selected :(");
	        }      
	    }

	    void GuestTargeted(int guestNumber)
	    {  
	        //Wraparound is handled here
	        if (guestNumber >= room.Guests.Length)
	        {
	            guestNumber -= room.Guests.Length;
	        } else if (guestNumber < 0)
	        {
	            guestNumber += room.Guests.Length;
	        }
	        GuestVO guest = room.Guests[guestNumber];
	        //Do they like the Tone?
	        if (party.playerHand[targetingRemark].tone == guest.disposition.like) //They like the tone
	        {
	            if (guest.isEnemy && GameData.playerReputationLevel >= 4)
	            {
	                ChangeGuestOpinion(guest, (int)((Random.Range(25, 36) * ReparteBonus()) * 1.25));
	            }
	            else
	            {
	                ChangeGuestOpinion(guest, (int)(Random.Range(25, 36) * ReparteBonus()));
	            }
	            AddRemarkToHand(); //Add a new Remark for Tone success           
	            party.currentPlayerConfidence = Mathf.Clamp(party.currentPlayerConfidence + 5, 5, party.maxPlayerConfidence); //Confidence Reward
	            guest.dispositionRevealed = true; // Reveal their Disposition to the Player (if concealed)
	        } else if (party.playerHand[targetingRemark].tone == guest.disposition.dislike) //They dislike the tone
	        {
	            if (!fascinatorEffect) //If the the Player doesn't have the Fascinator Accessory or its ability has already been used up
	            {
	                if (guest.isEnemy && GameData.playerReputationLevel >= 4)
	                {
	                    ChangeGuestOpinion(guest, (int)((Random.Range(-17, -11) * ReparteBonus()) * 1.25));
	                }
	                else
	                {
	                    ChangeGuestOpinion(guest, (int)(Random.Range(-17, -11) * ReparteBonus()));
	                }
	                party.currentPlayerConfidence = Mathf.Clamp(party.currentPlayerConfidence - 10, 0, party.maxPlayerConfidence); //Confidence Penalty
	            } else //If it hasn't yet, use up the ability and ignore the first Negative Comment Effect
	            {
	                fascinatorEffect = false;
	            }

	        } else //Neutral Tone
	        {
	            if(guest.isEnemy && GameData.playerReputationLevel >= 4)
	            {
	                ChangeGuestOpinion(guest, (int)((Random.Range(12, 18) * ReparteBonus())*1.25));
	            } else
	            {
	                ChangeGuestOpinion(guest, (int)(Random.Range(12, 18) * ReparteBonus()));
	            }
	        }
	        // Refill Interest of the Selected
	        guest.currentInterestTimer = guest.maxInterestTimer + 1; //Everyone loses one because of the Turn Timer

	        //If the Guest is an Enemy
	        if (guest.isEnemy)
	        {
	            //Hammering on Offended Guests gives confidence
	            if (guest.lockedInState == LockedInState.PutOff)
	            {
	                party.currentPlayerConfidence = Mathf.Clamp(party.currentPlayerConfidence + 10, 10, (int)(party.maxPlayerConfidence * 1.5));
	            }
	            //Check for Charmed Guests, this is necessary for the Attack Check below
	            GuestVO charmedGuest = null;
	            foreach (GuestVO g in room.Guests)
	            {
	                if (guest.lockedInState == LockedInState.Charmed)
	                {
	                    charmedGuest = g;
	                }
	            }
	            //The Actual Attack Check
	            EnemyAttackCheck(guest, charmedGuest);
	        }
	    }

	    public void GuestHighlighting(int guestNumber)
	    {
	        if (targetingMode)
	        {
	            switch (party.playerHand[targetingRemark].targetingProfileInt)
	            {
	                case 1:
	                    //No flip or failsafe version necessary, it's just one target
	                    GuestHighlight(guestNumber);
	                    break;
	                case 11:
	                    //Flip versions and failsafes
	                     if (!targetingFlipped)
	                     {
	                        GuestHighlight(guestNumber);
	                        GuestHighlight(guestNumber + 1);
	                     }else
	                     {
	                        GuestHighlight(guestNumber);
	                        GuestHighlight(guestNumber - 1);
	                     }
	                    break;
	                case 101:
	                    //Flip Versions and Failsafes
	                    if (!targetingFlipped)
	                    {
	                        GuestHighlight(guestNumber);
	                        GuestHighlight(guestNumber + 2);
	                    }
	                    else
	                    {
	                        GuestHighlight(guestNumber);
	                        GuestHighlight(guestNumber - 2);
	                    }
	                    break;
	                case 1011:
	                    //Flip version but no failsafes, as it covers all 4 Guests
	                    if (!targetingFlipped)
	                    {
	                        GuestHighlight(guestNumber);
	                        GuestHighlight(guestNumber + 2);
	                        GuestHighlight(guestNumber + 3);
	                    }
	                    else
	                    {
	                        GuestHighlight(guestNumber);
	                        GuestHighlight(guestNumber - 2);
	                        GuestHighlight(guestNumber - 3);
	                    }
	                    break;
	            }
	        }    
	    }

	    void GuestHighlight(int guestNumber)
	    {
	        if (guestNumber >= room.Guests.Length)
	        {
	            guestNumber -= room.Guests.Length;
	        }
	        else if (guestNumber < 0)
	        {
	            guestNumber += room.Guests.Length;
	        }
	        switch (guestNumber)
	        {
	            case 0:
	                guest0GuestImage.color = ReactionColor(0);
	                //guest0GuestImage.sprite = ReactionSprite(0);
	                break;
	            case 1:
	                guest1GuestImage.color = ReactionColor(1);
	                //guest1GuestImage.sprite = ReactionSprite(1);
	                break;
	            case 2:
	                //There might not be 3 or more Guests, so this check is to make sure nothing breaks
	                if (room.Guests.Length > 2)
	                {
	                    guest2GuestImage.color = ReactionColor(2);
	                    //guest2GuestImage.sprite = ReactionSprite(2);
	                }
	                break;
	            case 3:
	                //There might not be 4 Guests, so this check is to make sure nothing breaks
	                if (room.Guests.Length > 3)
	                {
	                    guest3GuestImage.color = ReactionColor(3);
	                    //guest3GuestImage.sprite = ReactionSprite(3);
	                }
	                break;
	        }
	    }
	    
	    //Called by the Pointer Exit button function of the Guest Images, used to reset Guest Images after the player mouses away from them
	    public void GuestUnhighlighting()
	    {
	        string dictionaryString;
	        for (int i = 0; i < guestImageList.Count; i++)
	        {
	            //Which Guest Image is Being Selected?
	            if (room.Guests[i].isFemale)
	            {
	                if (room.Guests[i].imageInt == 0)
	                {
	                    dictionaryString = "female0";
	                }
	                else
	                {
	                    dictionaryString = "female1";
	                }
	            }
	            else
	            {
	                if (room.Guests[i].imageInt == 0)
	                {
	                    dictionaryString = "male0";
	                }
	                else
	                {
	                    dictionaryString = "male1";
	                }
	            }
	            //What is the Locked In State of this Guest?
	            if (room.Guests[i].lockedInState == LockedInState.Charmed)
	            {
	                guestImageList[i].sprite = GuestImageSprintDictionaries[dictionaryString]["Charmed"];
	            } else if (room.Guests[i].lockedInState == LockedInState.Interested)
	            {
	                guestImageList[i].sprite = GuestImageSprintDictionaries[dictionaryString]["Neutral"];
	            } else
	            {
	                guestImageList[i].sprite = GuestImageSprintDictionaries[dictionaryString]["Put Off"];
	            }
	            guestImageList[i].color = Color.white;
	        }
	    }

	    void EnemyAttackCheck(GuestVO enemyGuest, GuestVO charmedGuest)
	    {
	        Debug.Log("Attack Started!");
	        int attackNumber = enemyGuest.AttackReaction(charmedGuest);
	        switch (attackNumber) {
	            case 0:
	                //Do Nothing
	                break;
	            case 1:
	                //1 = Monopolize Conversation (Lose a Turn)
	                EndTurn();
	                break;
	            case 2:
	                //2 = Rumor Monger (Lower the Opinion of all uncharmed Guests)
	                foreach (GuestVO g in room.Guests)
	                {
	                    if(g.lockedInState != LockedInState.Charmed)
	                    {
	                        g.currentOpinion -= 10;
	                    }
	                }
	                break;
	            case 3:
	                //3 = Belittle (Sap your Confidence)
	                party.currentPlayerConfidence -= 20;
	                break;
	            case 4:
	                //4 = Antagonize (Uncharm a Charmed Guest, if there is one)
	                charmedGuest.currentOpinion = 90;
	                charmedGuest.lockedInState =LockedInState.Interested;
	                break;
	        }
	        if (attackNumber != 0)
	        {
	            enemyGuest.attackTimerWaiting = true;
	            enemyGuest.attackNumber = attackNumber;
	        }
	        StartCoroutine(TimerAttackDisplay(enemyGuest));
	    }

	    IEnumerator TimerAttackDisplay(GuestVO g)
	    {
	        Debug.Log("Attack Timer Started!");
	        yield return new WaitForSeconds(0.75f);
	        g.attackTimerWaiting = false;
	    }

	    public float ReparteBonus()
	    {
	        if(room.TurnTimer/maxTurnTimer >= 0.5 && GameData.playerReputationLevel >= 2)
	        {
	            return 1.25f;
	        } else
	        {
	            return 1.0f;
	        }
	    }

	    //Unused at the moment, should there be a color shift with Guests when they're highlighted?
	    Color ReactionColor(int guestNumber)
	    {
	        if(!room.Guests[guestNumber].dispositionRevealed && party.currentPlayerIntoxication >= 50)
	        {
	            return Color.gray;
	        }
	        if (party.playerHand[targetingRemark].tone == room.Guests[guestNumber].disposition.like) //They like the tone
	        {
	            return Color.green;
	        }
	        else if (party.playerHand[targetingRemark].tone == room.Guests[guestNumber].disposition.dislike) //They dislike the tone
	        {
	            return Color.red;
	        }
	        else //Neutral Tone
	        {
	            return Color.gray;
	        }
	    }

	    Sprite ReactionSprite(int guestNumber)
	    {
	        string dictionaryString;
	        string reactionString;
	        //Which Guest Image is Being Selected?
	        if (room.Guests[guestNumber].isFemale)
	        {
	            if (room.Guests[guestNumber].imageInt == 0)
	            {
	                dictionaryString = "female0";
	            }
	            else
	            {
	                dictionaryString = "female1";
	            }
	        }
	        else
	        {
	            if (room.Guests[guestNumber].imageInt == 0)
	            {
	                dictionaryString = "male0";
	            }
	            else
	            {
	                dictionaryString = "male1";
	            }
	        }
	        //Which Guest Image State is Being Selected?
	        if (!room.Guests[guestNumber].dispositionRevealed && party.currentPlayerIntoxication >= 50) //Is the Player too trashed to notice at all?
	        {
	            reactionString = "Neutral";
	        }
	        if (party.playerHand[targetingRemark].tone == room.Guests[guestNumber].disposition.like) //They like the tone
	        {
	            reactionString = "Approve";
	        }
	        else if (party.playerHand[targetingRemark].tone == room.Guests[guestNumber].disposition.dislike) //They dislike the tone
	        {
	            reactionString = "Disapprove";
	        }
	        else //Neutral Tone
	        {
	            reactionString = "Neutral";
	        }
	        return GuestImageSprintDictionaries[dictionaryString][reactionString];
	    }

	    Sprite GuestStateSprite(int guestNumber)
	    {
	        string dictionaryString;
	        string reactionString;
	        //Which Guest Image is Being Selected?
	        if (room.Guests[guestNumber].isFemale)
	        {
	            if (room.Guests[guestNumber].imageInt == 0)
	            {
	                dictionaryString = "female0";
	            }
	            else
	            {
	                dictionaryString = "female1";
	            }
	        }
	        else
	        {
	            if (room.Guests[guestNumber].imageInt == 0)
	            {
	                dictionaryString = "male0";
	            }
	            else
	            {
	                dictionaryString = "male1";
	            }
	        }
	        //Which Guest Image State is Being Selected?
	        if(room.Guests[guestNumber].lockedInState == LockedInState.Charmed)
	        {
	            reactionString = "Charmed";
	        } else if (room.Guests[guestNumber].lockedInState == LockedInState.PutOff)
	        {
	            reactionString = "Put Off";
	        } else
	        {
	            if(room.Guests[guestNumber].currentOpinion >= 70)
	            {
	                reactionString = "Approve";
	            } else if (room.Guests[guestNumber].currentOpinion <= 30)
	            {
	                reactionString = "Disapprove";
	            } else
	            {
	                reactionString = "Neutral";
	            }
	        }
	        return GuestImageSprintDictionaries[dictionaryString][reactionString];
	    }

	    Color DispositionImageColor(int guest)
	    {
	        if (party.currentPlayerIntoxication >= 50 && room.Guests[guest].dispositionRevealed == false)
	        {
	            return Color.gray;
	        }
	        else
	        {
	            return GameData.dispositionList[room.Guests[guest].dispositionInt].color;
	        }
	    }

	    void EndTurn()
	    {
	        //Reset the Turn Timer
	        room.TurnTimer = maxTurnTimer;
	        turnTimerBar.value = room.TurnTimer / maxTurnTimer;

	        //Increment all the Guest Timers, issue Boredom Damage
	        foreach (GuestVO g in room.Guests)
	        {
	            g.currentInterestTimer = Mathf.Clamp(g.currentInterestTimer - 1, 0, g.maxInterestTimer);
	            if (g.currentInterestTimer <= 0 && g.lockedInState == LockedInState.Interested && !g.isEnemy) //Guest must not be locked in and must not be an Enemy, Enemies don't get bored they merely wait
	            {
	                ChangeGuestOpinion(g, -10);
	                if (g.currentOpinion <= 0)
	                {
	                    g.lockedInState = LockedInState.PutOff;
	                }
	            }
	        }
	        //Pause the Next Turn Timer
	        turnTimerActive = false;
	    }

	    void StockGuestImageDictionaries()
	    {
	        GuestImageSprintDictionaries.Add("female0", femaleGuestImageSpriteDictionary0);
	        GuestImageSprintDictionaries.Add("female1", femaleGuestImageSpriteDictionary1);
	        GuestImageSprintDictionaries.Add("male0", maleGuestImageSpriteDictionary0);
	        GuestImageSprintDictionaries.Add("male1", maleGuestImageSpriteDictionary1);

	        femaleGuestImageSpriteDictionary0.Add("Charmed", femaleGuestImage0Charmed);
	        femaleGuestImageSpriteDictionary0.Add("Approve", femaleGuestImage0Approve);
	        femaleGuestImageSpriteDictionary0.Add("Neutral", femaleGuestImage0Neutral);
	        femaleGuestImageSpriteDictionary0.Add("Disapprove", femaleGuestImage0PutOut);
	        femaleGuestImageSpriteDictionary0.Add("Put Off", femaleGuestImage0PutOut);

	        femaleGuestImageSpriteDictionary1.Add("Charmed", femaleGuestImage1Charmed);
	        femaleGuestImageSpriteDictionary1.Add("Approve", femaleGuestImage1Approve);
	        femaleGuestImageSpriteDictionary1.Add("Neutral", femaleGuestImage1Neutral);
	        femaleGuestImageSpriteDictionary1.Add("Disapprove", femaleGuestImage1PutOut);
	        femaleGuestImageSpriteDictionary1.Add("Put Off", femaleGuestImage1PutOut);

	        maleGuestImageSpriteDictionary0.Add("Charmed", maleGuestImage0Charmed);
	        maleGuestImageSpriteDictionary0.Add("Approve", maleGuestImage0Approve);
	        maleGuestImageSpriteDictionary0.Add("Neutral", maleGuestImage0Neutral);
	        maleGuestImageSpriteDictionary0.Add("Disapprove", maleGuestImage0PutOut);
	        maleGuestImageSpriteDictionary0.Add("Put Off", maleGuestImage0PutOut);

	        maleGuestImageSpriteDictionary1.Add("Charmed", maleGuestImage1Charmed);
	        maleGuestImageSpriteDictionary1.Add("Approve", maleGuestImage1Approve);
	        maleGuestImageSpriteDictionary1.Add("Neutral", maleGuestImage1Neutral);
	        maleGuestImageSpriteDictionary1.Add("Disapprove", maleGuestImage1PutOut);
	        maleGuestImageSpriteDictionary1.Add("Put Off", maleGuestImage1PutOut);
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

	    public void DrinkForConfidence()
	    {
	        if (party.currentPlayerDrinkAmount > 0 && party.currentPlayerConfidence != party.maxPlayerConfidence)
	        {
	            party.currentPlayerDrinkAmount--;
	            party.currentPlayerConfidence = Mathf.Clamp(party.currentPlayerConfidence + 20, 20, party.maxPlayerConfidence);
	            int drinkStrength = party.drinkStrength;
	            //Is the Player decent friends with the Military? If so, make them more alcohol tolerant!
	            if(GameData.factionList["Military"].ReputationLevel >= 3)
	            {
	                drinkStrength -= 3;
	            }
	            //Is the Player using the Snuff Box Accessory? If so, then decrease the Intoxicating Effects of Booze!
	            if (GameData.partyAccessory != null)
	            {
	                if (GameData.partyAccessory.Type == "Snuff Box")
	                {
	                    drinkStrength -= 5;
	                }
	            }        
	            party.currentPlayerIntoxication += drinkStrength;

	            //Conceal the dispositions of all the Guests in the room (only has effect if the Player is drunk)
	            foreach (GuestVO t in room.Guests)
	            {
	                t.dispositionRevealed = false;
	            }

	            //Check for Blacking Out
	            if (party.currentPlayerIntoxication >= party.maxPlayerIntoxication)
	            {
	                BlackOut();
	            }
	        }
	    }

	    void BlackOut()
	    {
	        Debug.Log("Blacking out!");
	        //Determine Random Effect
	        int effectSelection = Random.Range(1, 11);
	        string effect;
	        int effectAmount = 0;
	        switch (effectSelection)
	        {
	            case 1:
	                effect = "Reputation Loss";
	                effectAmount = Random.Range(20,51) * -1;
	                party.wonRewardsList.Add(new Reward(party, "Reputation", effectAmount));
	                break;
	            case 2:
	                effect = "Faction Reputation Loss";
	                effectAmount = Random.Range(20, 51) * -1;
	                party.wonRewardsList.Add(new Reward(party, "Faction Reputation", party.faction, effectAmount));
	                break;
	            case 3:
	                effect = "Outfit Novelty Loss";
	                effectAmount = Random.Range(20, 51);
	                OutfitInventory.PartyOutfit.novelty = Mathf.Clamp(OutfitInventory.PartyOutfit.novelty - effectAmount, 0, 100);
	                break;
	            case 4:
	                effect = "Outfit Ruined";
					OutfitInventory.personalInventory.Remove(OutfitInventory.PartyOutfit);
					OutfitInventory.PartyOutfit = null;
	                break;
	            case 5:
	                effect = "Accessory Ruined";
	                if(GameData.partyAccessory != null) //If the Player actually wore and Accessory to this Party
	                {
						DeWinterApp.SendMessage<ItemVO>(InventoryConsts.REMOVE_ITEM, GameData.partyAccessory);
	                } else
	                {
	                    effect = "Livre Lost";
	                    effectAmount = Random.Range(30, 61) * -1;
	                    party.wonRewardsList.Add(new Reward(party, "Livre", effectAmount));
	                }
	                break;
	            case 6:
	                effect = "Livre Lost";
	                effectAmount = Random.Range(30, 61) * -1;
	                party.wonRewardsList.Add(new Reward(party, "Livre", effectAmount));
	                break;
	            case 7:
	                effect = "New Enemy";
	                EnemyInventory.AddEnemy(new Enemy(party.faction));
	                break;
	            case 8:
	                effect = "Forgot All Gossip";
	                List<Reward> gossipList = new List<Reward>();
	                foreach (Reward r in party.wonRewardsList)
	                {
	                    if(r.Type() == "Gossip")
	                    {
	                        gossipList.Add(r);
	                    }
	                }
	                if (gossipList.Count != 0) //If the Player actually has Gossip to lose
	                {
	                    foreach (Reward r in gossipList)
	                    {
	                        party.wonRewardsList.Remove(r);
	                    }
	                } else //If they have no Gossip to Lose
	                {
	                    effect = "New Enemy";
	                    EnemyInventory.AddEnemy(new Enemy(party.faction));
	                }
	                break;
	            case 9:
	                effect = "No Effect";
	                break;
	            default:
	                effect = "Positive Effect";
	                break;
	        }
	        if (effect == "Positive Effect")
	        {
	            effectSelection = Random.Range(1, 6);
	            switch (effectSelection)
	            {
	                case 1:
	                    effect = "Reputation Gain";
	                    effectAmount = Random.Range(20, 51);
	                    party.wonRewardsList.Add(new Reward(party, "Reputation", effectAmount));
	                    break;
	                case 2:
	                    effect = "Faction Reputation Gain";
	                    effectAmount = Random.Range(20, 51);
	                    party.wonRewardsList.Add(new Reward(party, "Faction Reputation", party.faction, effectAmount));
	                    break;
	                case 3:
	                    effect = "Livre Gained";
	                    effectAmount = Random.Range(30, 61);
	                    party.wonRewardsList.Add(new Reward(party, "Livre", effectAmount));
	                    break;
	                case 4:
	                    effect = "New Gossip";
	                    party.wonRewardsList.Add(new Reward(party, "Gossip", 1));
	                    break;
	                default:
	                    effect = "Eliminated an Enemy";
	                    if(party.enemyList.Count == 0)
	                    {
	                        effect = "New Gossip";
	                        party.wonRewardsList.Add(new Reward(party, "Gossip", 1));
	                    } else
	                    {
	                        party.enemyList.RemoveAt(Random.Range(0, party.enemyList.Count));
	                    }
	                    break;
	            }  
	        }
	        party.blackOutEffect = effect;
	        party.blackOutEffectAmount = effectAmount;
	        Debug.Log("Black Out Effect Chosen! It's " + effect);
	        //Close Window
	        Destroy(gameObject);

	        //Send to After Party Report Screen
	        Debug.Log("Trying to go to the After Party Report Screen!");
	        party.blackOutEnding = true;
	        roomManager.partyManager.FinishTheParty();
			DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_AfterPartyReport");
	        Debug.Log("At the After Party Report Screen!");
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

	    void ChangeGuestOpinion(GuestVO guest, int amount)
	    {
	        if (guest.lockedInState == LockedInState.Interested) //Is this one locked in yet?
	        {
	            guest.currentOpinion += amount;
	        }
	        //Are they Charmed or Put Off?
	        if (guest.currentOpinion >= 100 && guest.lockedInState == LockedInState.Interested) //If they're not already Charmed then Player Hand is refilled once
	        {
	            guest.lockedInState = LockedInState.Charmed;
	            RefillPlayerHand();
	        }
	        else if (guest.currentOpinion <= 0 && guest.lockedInState == LockedInState.Interested) //If they're not already Put Off then Player Confidence is reduced by 30
	        {
	            guest.lockedInState = LockedInState.PutOff;
	            party.currentPlayerConfidence -= 30;
	        }
	        if (guest.lockedInState == LockedInState.Charmed) // If they're Charmed then Opinion is 100
	        {
	            guest.currentOpinion = 100;
	        }
	        else if (guest.lockedInState == LockedInState.PutOff) // If they're Put Off then Opinion is 0
	        {
	            guest.currentOpinion = 0;
	        }
	    }

	    void AddRemarkToHand()
	    {
	        if (party.playerHand.Count < 6) // This is one larger than it should be because Remarks are deducted after they're added
	        {
	            Remark remark = new Remark(party.lastTone, room.Guests.Length);
	            party.lastTone = remark.tone;
	            party.playerHand.Add(remark);
	        }
	    }

	    void RefillPlayerHand()
	    {
	        int numberOfCardsForRefill = 5 - party.playerHand.Count;
	        for (int i = 0; i < numberOfCardsForRefill; i++)
	        {
	            Remark remark = new Remark(party.lastTone, room.Guests.Length);
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

	    float InterestTimer(GuestVO guest)
	    {
	        return guest.currentInterestTimer/guest.maxInterestTimer;
	    }

	    string InterestState(GuestVO guest)
	    {
	        if (!guest.isEnemy)
	        {
	            if (guest.lockedInState == LockedInState.Charmed)
	            {
	                return "Charmed";
	            }
	            else if (guest.lockedInState == LockedInState.PutOff)
	            {
	                return "Put Off";
	            }
	            else if (guest.currentInterestTimer == 0)
	            {
	                return "BORED!";
	            }
	            else
	            {
	                return "Interested";
	            }
	        } else {
	            if (guest.attackTimerWaiting)
	            {
	                switch (guest.attackNumber)
	                {
	                    case 1:
	                        return "Monopolize the Conversation";
	                    case 2:
	                        return "Rumor Monger";
	                    case 3:
	                        return "Belittle";
	                    case 4:
	                        return "Antagonize";
	                    default:
	                        return "Attacking!";
	                }
	            } else {
	                if (guest.lockedInState == LockedInState.Charmed)
	                {
	                    return "Dazed";
	                }
	                else if (guest.lockedInState == LockedInState.PutOff)
	                {
	                    return "Offended";
	                }
	                else
	                {
	                    return "Plotting";
	                }
	            }         
	        }
	    }

	    void VictoryCheck()
	    {
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
	                foreach (Remark r in party.playerHand)
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

	    void ConfidenceCheck()
	    {
	        //Check to see if the Player has run out of Confidence
	        if (party.currentPlayerConfidence <= 0)
	        {
	            if (!party.tutorial) //If this is not the tutorial Party
	            {
	                //The Player loses a turn
	                party.turnsLeft--;
	                //The Player has their Confidence Reset
	                party.currentPlayerConfidence = party.startingPlayerConfidence / 2;
	                //The Player is relocated to the Entrance
	                roomManager.MovePlayerToEntrance();
	                //The Player's Reputation is Punished
	                int reputationLoss = 25;
	                int factionReputationLoss = 50;
	                GameData.reputationCount -= reputationLoss;
					DeWinterApp.SendMessage<AdjustValueVO>(new AdjustValueVO(party.faction, -factionReputationLoss));
	                //Explanation Screen Pop Up goes here
	                object[] objectStorage = new object[3];
	                objectStorage[0] = party;
	                objectStorage[1] = reputationLoss;
	                objectStorage[2] = factionReputationLoss;
	                screenFader.gameObject.SendMessage("CreateFailedConfidenceModal", objectStorage);
	                //The Player is pulled from the Work the Room session
	                Destroy(gameObject);
	            } else //The tutorial Party is a lot more forgiving
	            {
	                //The Player has their Confidence Reset
	                party.currentPlayerConfidence = party.startingPlayerConfidence;
	                //The Player is relocated to the Entrance
	                roomManager.MovePlayerToEntrance();
	                //The Player's Reputation is not Punished
	                int reputationLoss = 0;
	                int factionReputationLoss = 0;
	                //Explanation Screen Pop Up goes here
	                object[] objectStorage = new object[3];
	                objectStorage[0] = party;
	                objectStorage[1] = reputationLoss;
	                objectStorage[2] = factionReputationLoss;
	                screenFader.gameObject.SendMessage("CreateFailedConfidenceModal", objectStorage);
	                //The Player is pulled from the Work the Room session
	                Destroy(gameObject);
	            }  
	        }
	    }

	    public IEnumerator NextTurnTimerWait()
	    {
	        yield return new WaitForSeconds(0.75f);
	        turnTimerActive = true;
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
	    
	    void VisualizeGuests()
	    {
	        //---- Guest 0 ----
	        guest0InterestText.text = InterestState(room.Guests[0]);
	        if (room.Guests[0].isEnemy)
	        {
	            guest0InterestText.color = Color.red;
	        } else
	        {
	            guest0InterestText.color = Color.white;
	        }
	        guest0InterestBar.value = InterestTimer(room.Guests[0]);
	        guest0OpinionBar.value = (float)room.Guests[0].currentOpinion / 100;
	        guest0DispositionIcon.color = DispositionImageColor(0);
	        guest0GuestImage.sprite = GuestStateSprite(0);
	        //---- Guest 1 ----
	        guest1InterestText.text = InterestState(room.Guests[1]);
	        if (room.Guests[1].isEnemy)
	        {
	            guest1InterestText.color = Color.red;
	        }
	        else
	        {
	            guest1InterestText.color = Color.white;
	        }
	        guest1InterestBar.value = InterestTimer(room.Guests[1]);
	        guest1OpinionBar.value = (float)room.Guests[1].currentOpinion / 100;
	        guest1DispositionIcon.color = DispositionImageColor(1);
	        guest1GuestImage.sprite = GuestStateSprite(1);
	        //---- Guest 2 ---- 
	        //There might not be 3 Guests or more, so this check is to make sure nothing breaks
	        if(room.Guests.Length > 2)
	        {
	            guest2InterestText.text = InterestState(room.Guests[2]);
	            if (room.Guests[2].isEnemy)
	            {
	                guest2InterestText.color = Color.red;
	            }
	            else
	            {
	                guest2InterestText.color = Color.white;
	            }
	            guest2InterestBar.value = InterestTimer(room.Guests[2]);
	            guest2OpinionBar.value = (float)room.Guests[2].currentOpinion / 100;
	            guest2DispositionIcon.color = DispositionImageColor(2);
	            guest2GuestImage.sprite = GuestStateSprite(2);
	        }
	        //---- Guest 3 ----
	        //There might not be 4 Guests, so this check is to make sure nothing breaks
	        if (room.Guests.Length > 3)
	        {
	            guest3InterestText.text = InterestState(room.Guests[3]);
	            if (room.Guests[3].isEnemy)
	            {
	                guest3InterestText.color = Color.red;
	            }
	            else
	            {
	                guest3InterestText.color = Color.white;
	            }
	            guest3InterestBar.value = InterestTimer(room.Guests[3]);
	            guest3OpinionBar.value = (float)room.Guests[3].currentOpinion / 100;
	            guest3DispositionIcon.color = DispositionImageColor(3);
	            guest3GuestImage.sprite = GuestStateSprite(3);
	        }
	    }

		private GuestVO MakeRandomGuest()
		{
//			GuestVO guest = new GuestVO();
//			Random rnd = new Random();
//			guest.IsFemale = (rnd.Next(3) == 0);
//	        if (isFemale)
//	        {
//				title = GameData.femaleTitleList[Random.Range(0, GameData.femaleTitleList.Length)];
//				firstName = GameData.femaleFirstNameList[Random.Range(0, GameData.femaleFirstNameList.Length)];
//	        }
//	        else
//	        {
//	            title = GameData.maleTitleList[Random.Range(0, GameData.maleTitleList.Length)];
//				firstName = GameData.maleFirstNameList[Random.Range(0, GameData.maleFirstNameList.Length)];
//	        }
//			string lastName = GameData.lastNameList[Random.Range(0, GameData.lastNameList.Length)];
//	        return title + " " + firstName + " de " + lastName;
	    }

	    string GetLockedInState(GuestVO guest)
	    {
	    	if (guest.Opinion > 75)
	    		return "Charmed";
	    	if (guest.Opinion > 50)
	    		return "Interested";
	    	return "Put Off";
	    }
	}
}