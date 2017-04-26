using UnityEngine;
using System;
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
public GameObject playerVisual;
Text playerNameText;
Image playerImage;

		public KeyValuePair<BitArray, Sprite>[] RemarkSprites;
		public KeyValuePair<string, Sprite>[] Dispositions;
		public GuestSprite[] GuestSprites;
		public GuestViewMediator[] GuestViews;
		public RemarkViewMediator[] RemarkViews;
		public Image drinkBoozeButtonImage;

		private PartyModel _model;
		private RemarkVO _selectedRemark=null;
		private bool[] _targetted; // Targeted guests

		public Image reparteeIndicatorImage;

	    public Image readyGoPanel;
	    public Text readyGoText;

		// TODO: Passive Buff system
		private bool _fascinatorEffect; //The Fascinator Accessory lets the first negative comment go ignored during each Conversation

		void Awake()
		{
// TODO: Add these to the objects themselves
title = this.transform.Find("TitleText").GetComponent<Text>();
playerNameText = playerVisual.transform.Find("Name").GetComponent<Text>();
playerNameText.text = "Yvette";
drinkBoozeButtonImage = playerVisual.transform.Find("DrinkBoozeButton").GetComponent<Image>();

			_model = DeWinterApp.GetModel<PartyModel>();

			DeWinterApp.Subscribe<RoomVO>(HandleRoom);
			DeWinterApp.Subscribe<List<RemarkVO>>(HandleHand);
			DeWinterApp.Subscribe<float>(PartyMessages.START_TIMERS, HandleTimers);
			DeWinterApp.Subscribe<int>(GameConsts.INTOXICATION, HandleBooze);

			DeWinterApp.Subscribe<RemarkVO>(HandleRemarkSelected);
			DeWinterApp.Subscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleGuestTargeted);
			DeWinterApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleGuestSelected);
		}

		void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<RoomVO>(HandleRoom);
			DeWinterApp.Unsubscribe<List<RemarkVO>>(HandleHand);
			DeWinterApp.Unsubscribe<float>(PartyMessages.START_TIMERS, HandleTimers);
			DeWinterApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleBooze);

			DeWinterApp.Unsubscribe<RemarkVO>(HandleRemarkSelected);
			DeWinterApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleGuestTargeted);
			DeWinterApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleGuestSelected);
	    }

		// Use this for initialization
	    void Start()
	    {
			reparteeIndicatorImage.enabled = (GameData.playerReputationLevel >= 2);

	        //Ready Go Text
	        // TODO: Put all of this into the localization file
			string[] conversationIntroList = DeWinterApp.GetModel<PartyModel>().ConversationIntros;
	        readyGoText.text = conversationIntroList[new System.Random().Next(conversationIntroList.Length)];

	        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
	        // TODO: Passive buff system
			_fascinatorEffect = (GameData.partyAccessory != null && GameData.partyAccessory.Type == "Fascinator");
	    }

	    // Poll for input
	    void Update()
	    {
			if(Input.GetKeyDown(KeyCode.D))
	        {
	            DrinkForConfidence();
	        }
	        else if (Input.GetKeyDown(KeyCode.F))
	        {
	        	FlipTargets();
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

        // TODO: This can live in the text object itself
		private void HandleRoom(RoomVO room)
		{
			title.text = room.Name;
		}

		private void HandleHand(List<RemarkVO> Hand)
		{
			bool isRemark;
			for (int i = RemarkViews.Length-1; i>=0; i--)
			{
				isRemark = (i < Hand.Count);
				RemarkViews[i].enabled = isRemark;
				if (isRemark) RemarkViews[i].Remark = Hand[i];
			}
		}

		private void HandleBooze(int tox)
		{
			drinkBoozeButtonImage.color =  (tox > 0) ? Color.white : Color.gray;
		}

		private void HandleRemarkSelected(RemarkVO remark)
		{
			_selectedRemark = remark;
			foreach(GuestViewMediator guestView in GuestViews)
			{
				guestView.GetComponent<GraphicRaycaster>().enabled = (_selectedRemark != null);
			}
		}

		private void HandleGuestTargeted(GuestVO guest)
		{
			if (_targetted == null && _selectedRemark != null) // Early out if targets have already been set
			{
				if (guest != null) // The argument guest is the one being explicitly targeted
				{
					int index = Array.FindIndex(GuestViews, v => v.Guest == guest);
					if (index >= 0)
					{
						int numGuests = _model.Guests.Length;
						bool isTarget;
						_targetted = new bool[numGuests];
						_targetted[index] = true; // Set the initial target to true

						// Select the other targets in the profile
						for (int i = numGuests - 1; i > 0; i--)
						{
							isTarget = (1 & (_selectedRemark.Profile >> i)) == 1;
							_targetted[(i+index)%numGuests] = isTarget;
							if (isTarget) DeWinterApp.SendMessage<GuestVO>(PartyMessages.GUEST_TARGETED, _model.Guests[i]);
						}
					}
				}
				else // Argument guest is null. This happens when the user stops targetting a guest.
				{
					_targetted = null;
				}
			}
		}

		private void HandleGuestSelected(GuestVO guest)
		{
			KeyValuePair<GuestVO, RemarkVO> payload;
			for (int i=_targetted.Length-1; i>=0; i--)
			{
				if (_targetted[i])
				{
					// Let a command do the heavy lifting
					payload = new KeyValuePair<GuestVO, RemarkVO>(_model.Guests[i], _selectedRemark);
					DeWinterApp.SendMessage<KeyValuePair<GuestVO, RemarkVO>>(payload);
				}
			}
			DeWinterApp.SendMessage<RemarkVO>(null); // Deselect the remark
			DeWinterApp.SendMessage<GuestVO[]>(_model.Guests);
		}

		public void FlipTargets()
	    {
			int flip = _selectedRemark.Profile;
			_selectedRemark.Profile = 0;
	    	while (flip > 0)
	    	{
	    		_selectedRemark.Profile <<= 1;
	    		_selectedRemark.Profile += (flip & 1);
				flip >>= 1;
	    	}
	    	DeWinterApp.SendMessage<RemarkVO>(_selectedRemark);
	    }

		private void HandleTimers(float seconds)
		{
			StartCoroutine(TimerCoroutine(seconds));
		}

		IEnumerator TimerCoroutine(float seconds)
		{
			float t = seconds;
			while (t > 0)
			{
				t -= Time.deltaTime;

				yield return null;
			}
			EndTurn(t/seconds);
		}

//	        VictoryCheck();
//	        ConfidenceCheck();

/*
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
*/



	    
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

	    void EndTurn(float percentComplete=0.0f)
	    {
	    	StopAllCoroutines();
			if (percentComplete >= 0.5f)
		    	DeWinterApp.SendMessage(PartyMessages.REPARTEE_BONUS);

	        //Signal End of turn (resets timers, etc)
			DeWinterApp.SendMessage(PartyMessages.END_TURN);
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
					DeWinterApp.AdjustValue<>(new AdjustValueVO(party.faction, -factionReputationLoss));
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