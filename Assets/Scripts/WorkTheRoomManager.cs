using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DeWinter;

public class WorkTheRoomManager : MonoBehaviour
{
    public SceneFadeInOut screenFader;
    public RoomManager roomManager;

    public RoomVO room;
    Text title;
    public bool isAmbush;

    List<Image> guestImageList = new List<Image>();

    public GameObject playerVisual;
    Text playerNameText;
    Text playerConfidenceText;
    Scrollbar playerConfidenceBar;
    Text playerIntoxicationText;
    Image playerImage;

    public Text playerDrinkAmountText;
    public Image drinkBoozeButtonImage;

    public Scrollbar turnTimerBar;
    bool turnTimerActive;
    public Image reparteeIndicatorImage;

    List<GameObject> guestVisualList;

    public GameObject guest0Visual;
    public Text guest0NameText;
    public Image guest0GuestImage;
    public Text guest0InterestText;
    public Scrollbar guest0InterestBar;
    public Image guest0InterestBarImage;
    public Text guest0OpinionText;
    public Scrollbar guest0OpinionBar;
    public Image guest0OpinionBarImage;
    public Image guest0DispositionIcon;

    public GameObject guest1Visual;
    public Text guest1NameText;
    public Image guest1GuestImage;
    public Text guest1InterestText;
    public Scrollbar guest1InterestBar;
    public Image guest1InterestBarImage;
    public Text guest1OpinionText;
    public Scrollbar guest1OpinionBar;
    public Image guest1OpinionBarImage;
    public Image guest1DispositionIcon;

    public GameObject guest2Visual;
    public Text guest2NameText;
    public Image guest2GuestImage;
    public Text guest2InterestText;
    public Scrollbar guest2InterestBar;
    public Image guest2InterestBarImage;
    public Text guest2OpinionText;
    public Scrollbar guest2OpinionBar;
    public Image guest2OpinionBarImage;
    public Image guest2DispositionIcon;

    public GameObject guest3Visual;
    public Text guest3NameText;
    public Image guest3GuestImage;
    public Text guest3InterestText;
    public Scrollbar guest3InterestBar;
    public Image guest3InterestBarImage;
    public Text guest3OpinionText;
    public Scrollbar guest3OpinionBar;
    public Image guest3OpinionBarImage;
    public Image guest3DispositionIcon;

    public Sprite femaleGuestImage0Charmed;
    public Sprite femaleGuestImage0Approve;
    public Sprite femaleGuestImage0Neutral;
    public Sprite femaleGuestImage0PutOut;

    public Sprite femaleGuestImage1Charmed;
    public Sprite femaleGuestImage1Approve;
    public Sprite femaleGuestImage1Neutral;
    public Sprite femaleGuestImage1PutOut;

    public Sprite maleGuestImage0Charmed;
    public Sprite maleGuestImage0Approve;
    public Sprite maleGuestImage0Neutral;
    public Sprite maleGuestImage0PutOut;

    public Sprite maleGuestImage1Charmed;
    public Sprite maleGuestImage1Approve;
    public Sprite maleGuestImage1Neutral;
    public Sprite maleGuestImage1PutOut;

    Dictionary<string, Dictionary<string, Sprite>> GuestImageSprintDictionaries = new Dictionary<string, Dictionary<string, Sprite>>();
    Dictionary<string, Sprite> femaleGuestImageSpriteDictionary0 = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> femaleGuestImageSpriteDictionary1 = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> maleGuestImageSpriteDictionary0 = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> maleGuestImageSpriteDictionary1 = new Dictionary<string, Sprite>();

    private List<RemarkSlot> remarkSlotList;

    public GameObject remarkSlot0;
    public RemarkSlot remarkSlot0RemarkSlot;
    Image remarkSlot0TargetingImage;
    Image remarkSlot0DispositionIcon;

    public GameObject remarkSlot1;
    public RemarkSlot remarkSlot1RemarkSlot;
    Image remarkSlot1TargetingImage;
    Image remarkSlot1DispositionIcon;

    public GameObject remarkSlot2;
    public RemarkSlot remarkSlot2RemarkSlot;
    Image remarkSlot2TargetingImage;
    Image remarkSlot2DispositionIcon;

    public GameObject remarkSlot3;
    public RemarkSlot remarkSlot3RemarkSlot;
    Image remarkSlot3TargetingImage;
    Image remarkSlot3DispositionIcon;

    public GameObject remarkSlot4;
    public RemarkSlot remarkSlot4RemarkSlot;
    Image remarkSlot4TargetingImage;
    Image remarkSlot4DispositionIcon;

    public Sprite targetingProfile1;
    public Sprite targetingProfile11;
    public Sprite targetingProfile101;
    public Sprite targetingProfile1001;
    public Sprite targetingProfile1011;

    bool targetingMode = false; //Are we selecting an target for a remark right now?
    int targetingRemark = -1;
    bool targetingFlipped = false; //Has targetting been flipped? Necessary for the Targeting Profiles

    bool conversationStarted = false;
    public Image readyGoPanel;
    public Text readyGoText;

    int maxTurnTimer=5;

    bool fascinatorEffect; //The Fascinator Accessory lets the first negative comment go ignored during each Conversation

    private Party party
    {
    	get { return DeWinterApp.GetModel<PartyModel>().Party; }
    }

    // Use this for initialization
    void Start()
    {
        screenFader = this.transform.parent.GetComponent<SceneFadeInOut>();

        title = this.transform.Find("TitleText").GetComponent<Text>();
        title.text = room.Name;

        //Visualize the Player
        playerNameText = playerVisual.transform.Find("Name").GetComponent<Text>();
        playerNameText.text = "Yvette";
        playerConfidenceText = playerVisual.transform.Find("ConfidenceCounter").GetComponent<Text>();
        playerConfidenceText.text = "Confidence: " + party.currentPlayerConfidence + "/" + party.maxPlayerConfidence;
        playerConfidenceBar = playerVisual.transform.Find("ConfidenceBar").GetComponent<Scrollbar>();
        playerConfidenceBar.value = (float)party.currentPlayerConfidence / party.maxPlayerConfidence;
        playerIntoxicationText = playerVisual.transform.Find("DrinkBoozeButton").Find("IntoxicationCounter").GetComponent<Text>();
        playerIntoxicationText.text = "Intoxication: " + party.currentPlayerIntoxication + "/" + party.maxPlayerIntoxication;
        drinkBoozeButtonImage = playerVisual.transform.Find("DrinkBoozeButton").GetComponent<Image>();

        //Stock the Guest Images Lists
        StockGuestImageDictionaries();

        //Set Up the Guests
        SetUpGuests();

		DeWinterApp.Subscribe(PartyMessages.START_TIMERS, HandleStartTimers);

        //Generate the Remarks
        if (isAmbush)
        {
            if (party.playerHand.Count == 5)
            {
                party.playerHand.RemoveAt(4);
                party.playerHand.RemoveAt(3);
            }
            if (party.playerHand.Count == 4)
            {
                party.playerHand.RemoveAt(3);
            }
            party.playerHand.Add(new Remark("ambush", room.Guests.Length));
			party.playerHand.Add(new Remark("ambush", room.Guests.Length));
        }

        //Set Up Remarks
        remarkSlotList = new List<RemarkSlot>();
        remarkSlotList.Add(remarkSlot0RemarkSlot);
        remarkSlotList.Add(remarkSlot1RemarkSlot);
        remarkSlotList.Add(remarkSlot2RemarkSlot);
        remarkSlotList.Add(remarkSlot3RemarkSlot);
        remarkSlotList.Add(remarkSlot4RemarkSlot);  

        //Turn Timer
        turnTimerBar.value = room.TurnTimer / maxTurnTimer;
        if(GameData.playerReputationLevel >= 2)
        {
            reparteeIndicatorImage.color = Color.green;
        } else
        {
            reparteeIndicatorImage.color = Color.clear;
        }

        //Ready Go Text
		string[] conversationIntroList = DeWinterApp.GetModel<PartyModel>().ConversationIntros;
        readyGoText.text = conversationIntroList[Random.Range(0, conversationIntroList.Length)];
        //Tutorial Pop-Up? Only used in the tutorial Room
        // This can be made less hacky by registering the dialog in a command triggered by entering a room
        if (room.IsTutorial)
        {
            screenFader.gameObject.SendMessage("CreateWorkTheRoomTutorialPopUp", this);
        } else
        {
        	DeWinterApp.SendMessage(PartyMessages.START_TIMERS);
        }

        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
        if(GameData.partyAccessory != null)
        {
			if (GameData.partyAccessory.Type == "Fascinator")
            {
                fascinatorEffect = true;
            }
            else
            {
                fascinatorEffect = false;
            }
        }       
    }

    // Update is called once per frame
    void Update()
    {
        //Visualize the Player---------------
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
        if (party.currentPlayerDrinkAmount > 0)
        {
            drinkBoozeButtonImage.color = Color.white;
        } else
        {
            drinkBoozeButtonImage.color = Color.gray;
        }
        
        //Visualize Guests--------------------
        VisualizeGuests();

        //Interest Timers are only displayed when the Guest isn't locked in
        InterestTimersDisplayCheck();

        //Visualizing the Remarks--------------
        VisualizeRemarks();
        
        //Hotkey Listeners---------------------
        ListenForHotkeys();

        //Turn Timer
        if(conversationStarted && turnTimerActive)
        {
            room.TurnTimer -= Time.deltaTime;
            turnTimerBar.value = room.TurnTimer / maxTurnTimer;
            if (room.TurnTimer <= 0)
            {
                EndTurn();
            }
        } else if (conversationStarted && !turnTimerActive)
        {           
            StartCoroutine(NextTurnTimerWait());
        }        

        //Victory and Defeat Checks------------ 
        VictoryCheck();
        ConfidenceCheck();
    }

    void OnDestroy()
    {
		DeWinterApp.Unsubscribe(PartyMessages.START_TIMERS, HandleStartTimers);
    }

    void SetUpGuests()
    {
        //---- Set Up Guest 0 ----
        guest0NameText.text = room.Guests[0].Name;
        guest0GuestImage.sprite = GuestStateSprite(0);
        guest0DispositionIcon.color = DispositionImageColor(0);
        if (room.Guests[0].isEnemy)
        {
            guest0NameText.color = Color.red;
            guest0InterestBarImage.color = Color.clear;
        }
        else
        {
            guest0NameText.color = Color.white;
            guest0InterestBarImage.color = Color.white;
        }
        guestImageList.Add(guest0GuestImage);

        //---- Set Up Guest 1 ----
        guest1NameText.text = room.Guests[1].Name; 
        guest1GuestImage.sprite = GuestStateSprite(1);    
        guest1DispositionIcon.color = DispositionImageColor(1);
        if (room.Guests[1].isEnemy)
        {
            guest1NameText.color = Color.red;
            guest1InterestBarImage.color = Color.clear;
        }
        else
        {
            guest1NameText.color = Color.white;
            guest1InterestBarImage.color = Color.white;
        }
        guestImageList.Add(guest1GuestImage);

        //---- Set Up Guest 2 ----
        if (room.Guests.Length > 2)
        {
            guest2NameText.text = room.Guests[2].Name;
            guest2GuestImage.sprite = GuestStateSprite(2);
            guest2DispositionIcon.color = DispositionImageColor(2);
            if (room.Guests[2].isEnemy)
            {
                guest2NameText.color = Color.red;
                guest2InterestBarImage.color = Color.clear;
            }
            else
            {
                guest2NameText.color = Color.white;
                guest2InterestBarImage.color = Color.white;
            }
            guestImageList.Add(guest2GuestImage);
        } else
        {
            guest2NameText.text = "";
            guest2GuestImage.sprite = null;
            guest2GuestImage.color = Color.clear;
            guest2DispositionIcon.color = Color.clear;
            guest2InterestBar.image.color = Color.clear;
            guest2InterestBarImage.color = Color.clear;
            guest2InterestText.color = Color.clear;
            guest2OpinionText.color = Color.clear;
            guest2OpinionBarImage.color = Color.clear;
            guest2OpinionBar.image.color = Color.clear;
        }

        //---- Set Up Guest 3 ----
        if (room.Guests.Length > 3)
        {
            guest3NameText.text = room.Guests[3].Name;
            guest3GuestImage.sprite = GuestStateSprite(3);
            guest3DispositionIcon.color = DispositionImageColor(3);
            if (room.Guests[3].isEnemy)
            {
                guest3NameText.color = Color.red;
                guest3InterestBarImage.color = Color.clear;
            }
            else
            {
                guest3NameText.color = Color.white;
                guest3InterestBarImage.color = Color.white;
            }
            guestImageList.Add(guest3GuestImage);
        }
        else
        {
            guest3NameText.text = "";
            guest3GuestImage.sprite = null;
            guest3GuestImage.color = Color.clear;
            guest3DispositionIcon.color = Color.clear;
            guest3InterestBarImage.color = Color.clear;
            guest3InterestBar.image.color = Color.clear;
            guest3InterestText.color = Color.clear;
            guest3OpinionText.color = Color.clear;
            guest3OpinionBarImage.color = Color.clear;
            guest3OpinionBar.image.color = Color.clear;
        }
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
        Guest guest = room.Guests[guestNumber];
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
            Guest charmedGuest = null;
            foreach (Guest g in room.Guests)
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

    void EnemyAttackCheck(Guest enemyGuest, Guest charmedGuest)
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
                foreach (Guest g in room.Guests)
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

    IEnumerator TimerAttackDisplay(Guest g)
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
        foreach (Guest g in room.Guests)
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
            foreach (Guest t in room.Guests)
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

    void ChangeGuestOpinion(Guest guest, int amount)
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

    float InterestTimer(Guest guest)
    {
        return guest.currentInterestTimer/guest.maxInterestTimer;
    }

    string InterestState(Guest guest)
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

		foreach (Guest g in room.Guests)
        {
        	switch (g.lockedInState)
        	{
            	case LockedInState.Charmed:
            		charmedAmount++;
            		break;
            	case LockedInState.PutOff:
            		putOffAmount++;
            		break;
            }
        }

        //If the Conversation is Over
        if (charmedAmount + putOffAmount == room.Guests.Length)
        {
	        room.Cleared = true;

            //Remove the Ambush Cards (If present)
            party.playerHand.RemoveAll(r => r.ambushRemark);

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
            Dictionary<string, string> subs = new Dictionary<string, string>(){
				{"$NUMCHARMED",charmedAmount.ToString()},
				{"$NUMPUTOFF",putOffAmount.ToString()},
				{"$REWARD",givenReward.Name()}};
            DeWinterApp.OpenMessageDialog(DialogConsts.CONVERSATION_OVER_DIALOG, subs);

            //Close the Window
            Destroy(gameObject);
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

				Dictionary<string, string> subs = new Dictionary<string, string>(){
				{"$FACTIONREPUTATION",factionReputationLoss.ToString()},
				{"$FACTION",party.faction},
				{"$REPUTATION",reputationLoss.ToString()}};
           		DeWinterApp.OpenMessageDialog(DialogConsts.OUT_OF_CONFIDENCE_DIALOG, subs);

                //The Player is pulled from the Work the Room session
                Destroy(gameObject);
            } else //The tutorial Party is a lot more forgiving
            {
                //The Player has their Confidence Reset
                party.currentPlayerConfidence = party.startingPlayerConfidence;
                //The Player is relocated to the Entrance
                roomManager.MovePlayerToEntrance();
                //The Player's Reputation is not Punished
	            DeWinterApp.OpenMessageDialog(DialogConsts.OUT_OF_CONFIDENCE_TUTORIAL_DIALOG);
                //The Player is pulled from the Work the Room session
                Destroy(gameObject);
            }  
        }
    }

    void InterestTimersDisplayCheck()
    {
        //------------- Guest 0 -------------
        if (room.Guests[0].lockedInState != LockedInState.Interested || room.Guests[0].isEnemy)
        {
            guest0InterestBar.image.color = Color.clear;
            guest0InterestBarImage.color = Color.clear;
        }
        else
        {
            guest0InterestBar.image.color = Color.white;
            guest0InterestBarImage.color = Color.white;
        }

        //------------- Guest 1 -------------
        if (room.Guests[1].lockedInState != LockedInState.Interested || room.Guests[1].isEnemy)
        {
            guest1InterestBar.image.color = Color.clear;
            guest1InterestBarImage.color = Color.clear;
        }
        else
        {
            guest1InterestBar.image.color = Color.white;
            guest1InterestBarImage.color = Color.white;
        }

        //------------- Guest 2 -------------
        //There might not be 3 Guests or more, so this check is to make sure nothing breaks
        if (room.Guests.Length > 2)
        {
            if (room.Guests[2].lockedInState != LockedInState.Interested || room.Guests[2].isEnemy)
            {
                guest2InterestBar.image.color = Color.clear;
                guest2InterestBarImage.color = Color.clear;
            }
            else
            {
                guest2InterestBar.image.color = Color.white;
                guest2InterestBarImage.color = Color.white;
            }
        }

        //------------- Guest 3 -------------
        //There might not be 4 Guests or more, so this check is to make sure nothing breaks
        if (room.Guests.Length > 3)
        {
            if (room.Guests[3].lockedInState != LockedInState.Interested || room.Guests[3].isEnemy)
            {
                guest3InterestBar.image.color = Color.clear;
                guest3InterestBarImage.color = Color.clear;
            }
            else
            {
                guest3InterestBar.image.color = Color.white;
                guest3InterestBarImage.color = Color.white;
            }
        }
    }

	private void HandleStartTimers()
	{
		StartCoroutine(ConversationStartTimerWait());
	}

    public IEnumerator ConversationStartTimerWait()
    {
        Debug.Log("Ready? Go! Timer Started!");
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

    void ListenForHotkeys()
    {
        if(Input.GetKeyDown(KeyCode.D))
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
}

    
