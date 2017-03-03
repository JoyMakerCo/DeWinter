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
    LevelManager levelManager;

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
    float currentTurnTimer = 5;
    float maxTurnTimer = 5;
    bool turnTimerActive;
    public Image reparteeIndicatorImage;

    List<GameObject> guestVisualList;

    public GameObject guest0Visual;
    public Text guest0NameText;
    public Image guest0GuestImage;
    public Text guest0InterestText;
    public Scrollbar guest0InterestBar;
    public Image guest0InterestBarBackground;
    public Scrollbar guest0OpinionBar;
    public Image guest0DispositionIcon;

    public GameObject guest1Visual;
    public Text guest1NameText;
    public Image guest1GuestImage;
    public Text guest1InterestText;
    public Scrollbar guest1InterestBar;
    public Image guest1InterestBarBackground;
    public Scrollbar guest1OpinionBar;
    public Image guest1DispositionIcon;

    public GameObject guest2Visual;
    public Text guest2NameText;
    public Image guest2GuestImage;
    public Text guest2InterestText;
    public Scrollbar guest2InterestBar;
    public Image guest2InterestBarBackground;
    public Scrollbar guest2OpinionBar;
    public Image guest2DispositionIcon;

    public GameObject guest3Visual;
    public Text guest3NameText;
    public Image guest3GuestImage;
    public Text guest3InterestText;
    public Scrollbar guest3InterestBar;
    public Image guest3InterestBarBackground;
    public Scrollbar guest3OpinionBar;
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

    bool fascinatorEffect; //The Fascinator Accessory lets the first negative comment go ignored during each Conversation

    private PartyModel _partyModel;
	private MapModel _mapModel;
	private InventoryModel _inventoryModel;

    public RoomVO Room
    {
    	get { return _mapModel.Room; }
    	set { _mapModel.Room = value; }
    }

	public Party Party
    {
    	get { return _partyModel.Party; }
    }

    // Use this for initialization
    void Start()
    {
		_partyModel = DeWinterApp.GetModel<PartyModel>();
		_mapModel = DeWinterApp.GetModel<MapModel>();
		_inventoryModel = DeWinterApp.GetModel<InventoryModel>();

        screenFader = this.transform.parent.GetComponent<SceneFadeInOut>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        title = this.transform.Find("TitleText").GetComponent<Text>();
        title.text = Room.Name;

        //Visualize the Player
        playerNameText = playerVisual.transform.Find("Name").GetComponent<Text>();
        playerNameText.text = "Lady De Winter";
        playerConfidenceText = playerVisual.transform.Find("ConfidenceCounter").GetComponent<Text>();
        playerConfidenceText.text = "Confidence: " + Party.currentPlayerConfidence + "/" + Party.maxPlayerConfidence;
        playerConfidenceBar = playerVisual.transform.Find("ConfidenceBar").GetComponent<Scrollbar>();
        playerConfidenceBar.value = (float)Party.currentPlayerConfidence / Party.maxPlayerConfidence;
        playerIntoxicationText = playerVisual.transform.Find("DrinkBoozeButton").Find("IntoxicationCounter").GetComponent<Text>();
        playerIntoxicationText.text = "Intoxication: " + Party.currentPlayerIntoxication + "/" + Party.maxPlayerIntoxication;
        drinkBoozeButtonImage = playerVisual.transform.Find("DrinkBoozeButton").GetComponent<Image>();

        //Stock the Guest Images Lists
        StockGuestImageDictionaries();

        //Set Up the Guests
        SetUpGuests();

        //Generate the Remarks
        if (isAmbush)
        {
            if (Party.playerHand.Count == 5)
            {
                Party.playerHand.RemoveAt(4);
                Party.playerHand.RemoveAt(3);
            }
            if (Party.playerHand.Count == 4)
            {
                Party.playerHand.RemoveAt(3);
            }
            Party.playerHand.Add(new Remark("ambush"));
            Party.playerHand.Add(new Remark("ambush"));
        }

        //Set Up Remarks
        remarkSlotList = new List<RemarkSlot>();
        remarkSlotList.Add(remarkSlot0RemarkSlot);
        remarkSlotList.Add(remarkSlot1RemarkSlot);
        remarkSlotList.Add(remarkSlot2RemarkSlot);
        remarkSlotList.Add(remarkSlot3RemarkSlot);
        remarkSlotList.Add(remarkSlot4RemarkSlot);

        //Turn Timer
        turnTimerBar.value = currentTurnTimer / maxTurnTimer;
        if(GameData.playerReputationLevel >= 2)
        {
            reparteeIndicatorImage.color = Color.green;
        } else
        {
            reparteeIndicatorImage.color = Color.clear;
        }

        //Ready Go Text
		readyGoText.text = _partyModel.ConversationIntros[Random.Range(0, _partyModel.ConversationIntros.Length)];
        StartCoroutine(ConversationStartTimerWait());

        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
        ItemVO accessory;
		fascinatorEffect = (_inventoryModel.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory.Type == "Fascinator");
    }

    // Update is called once per frame
    void Update()
    {
        //Visualize the Player---------------
        playerConfidenceText.text = "Confidence: " + Party.currentPlayerConfidence + "/" + Party.maxPlayerConfidence;
        playerConfidenceBar.value = (float)Party.currentPlayerConfidence / Party.maxPlayerConfidence;
        if (Party.currentPlayerConfidence <= 25)
        {
            playerConfidenceText.color = Color.red;
        }
        else
        {
            playerConfidenceText.color = Color.white;
        }
        playerIntoxicationText.text = "Intoxication: " + Party.currentPlayerIntoxication + "/" + Party.maxPlayerIntoxication;
        playerDrinkAmountText.text = "Booze Glass: " + Party.currentPlayerDrinkAmount + "/" + Party.maxPlayerDrinkAmount;
        if (Party.currentPlayerDrinkAmount > 0)
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
            currentTurnTimer -= Time.deltaTime;
            turnTimerBar.value = currentTurnTimer / maxTurnTimer;
            if (currentTurnTimer <= 0)
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

    void SetUpGuests()
    {

        //Set Up Guest 0
        guest0NameText.text = Room.Guests[0].Name;
        guest0GuestImage.sprite = GenerateGuestImage(0);
        guest0DispositionIcon.color = DispositionImageColor(0);
        if (Room.Guests[0].isEnemy)
        {
            guest0NameText.color = Color.red;
            guest0InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest0NameText.color = Color.white;
            guest0InterestBarBackground.color = Color.white;
        }

        //Set Up Guest 1
        guest1NameText.text = Room.Guests[1].Name; 
        guest1GuestImage.sprite = GenerateGuestImage(1);    
        guest1DispositionIcon.color = DispositionImageColor(1);
        if (Room.Guests[1].isEnemy)
        {
            guest1NameText.color = Color.red;
            guest1InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest1NameText.color = Color.white;
            guest1InterestBarBackground.color = Color.white;
        }

        //Set Up Guest 2
        guest2NameText.text = Room.Guests[2].Name;
        guest2GuestImage.sprite = GenerateGuestImage(2);   
        guest2DispositionIcon.color = DispositionImageColor(2);
        if (Room.Guests[2].isEnemy)
        {
            guest2NameText.color = Color.red;
            guest2InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest2NameText.color = Color.white;
            guest2InterestBarBackground.color = Color.white;
        }

        //Set Up Guest 3
        guest3NameText.text = Room.Guests[3].Name;
        guest3GuestImage.sprite = GenerateGuestImage(3);    
        guest3DispositionIcon.color = DispositionImageColor(3);
        if (Room.Guests[3].isEnemy)
        {
            guest3NameText.color = Color.red;
            guest3InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest3NameText.color = Color.white;
            guest3InterestBarBackground.color = Color.white;
        }

        //Put the Guest Images in the list (for Highlighting Purposes)
        guestImageList.Add(guest0GuestImage);
        guestImageList.Add(guest1GuestImage);
        guestImageList.Add(guest2GuestImage);
        guestImageList.Add(guest3GuestImage);
    }

    public void StartTargeting(int selectedRemark)
    {
        if (conversationStarted)
        {
            if (Party.playerHand[selectedRemark] != null && !Party.playerHand[selectedRemark].ambushRemark)
            {
                targetingMode = true;
                targetingRemark = selectedRemark;
                //Debug.Log("Remark Tone: " + GameData.dispositionList[Party.playerHand[selectedRemark].toneInt].like + " " + Party.playerHand[selectedRemark].toneInt);
                //Debug.Log("Remark Targeting Profile: " + Party.playerHand[selectedRemark].targetingProfileInt);
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
            switch (Party.playerHand[targetingRemark].targetingProfileInt)
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
            Party.playerHand.RemoveAt(targetingRemark);
            targetingRemark = -1;
            targetingMode = false;
            if (Party.playerHand.Count == 0)
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
        if (guestNumber >= Room.Guests.Length)
        {
            guestNumber -= Room.Guests.Length;
        } else if (guestNumber < 0)
        {
            guestNumber += Room.Guests.Length;
        }
        Guest guest = Room.Guests[guestNumber];
        //Do they like the Tone?
        if (Party.playerHand[targetingRemark].tone == guest.disposition.like) //They like the tone
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
            Party.currentPlayerConfidence = Mathf.Clamp(Party.currentPlayerConfidence + 5, 5, Party.maxPlayerConfidence); //Confidence Reward
            guest.dispositionRevealed = true; // Reveal their Disposition to the Player (if concealed)
        } else if (Party.playerHand[targetingRemark].tone == guest.disposition.dislike) //They dislike the tone
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
                Party.currentPlayerConfidence = Mathf.Clamp(Party.currentPlayerConfidence - 10, 0, Party.maxPlayerConfidence); //Confidence Penalty
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
            if (guest.lockedInState == -1)
            {
                Party.currentPlayerConfidence = Mathf.Clamp(Party.currentPlayerConfidence + 10, 10, (int)(Party.maxPlayerConfidence * 1.5));
            }
            //Check for Charmed Guests, this is necessary for the Attack Check below
            Guest charmedGuest = null;
            foreach (Guest g in Room.Guests)
            {
                if (g.lockedInState == 1)
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
            switch (Party.playerHand[targetingRemark].targetingProfileInt)
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
        if (guestNumber >= Room.Guests.Length)
        {
            guestNumber -= Room.Guests.Length;
        }
        else if (guestNumber < 0)
        {
            guestNumber += Room.Guests.Length;
        }
        switch (guestNumber)
        {
            case 0:
                //guest0GuestImage.color = ReactionColor(0);
                guest0GuestImage.sprite = ReactionSprite(0);
                break;
            case 1:
                //guest1GuestImage.color = ReactionColor(1);
                guest1GuestImage.sprite = ReactionSprite(1);
                break;
            case 2:
                //guest2GuestImage.color = ReactionColor(2);
                guest2GuestImage.sprite = ReactionSprite(2);
                break;
            case 3:
                //guest3GuestImage.color = ReactionColor(3);
                guest3GuestImage.sprite = ReactionSprite(3);
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
            if (Room.Guests[i].isFemale)
            {
                if (Room.Guests[i].imageInt == 0)
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
                if (Room.Guests[i].imageInt == 0)
                {
                    dictionaryString = "male0";
                }
                else
                {
                    dictionaryString = "male1";
                }
            }
            //What is the Locked In State of this Guest?
            if (Room.Guests[i].lockedInState == 1)
            {
                guestImageList[i].sprite = GuestImageSprintDictionaries[dictionaryString]["Charmed"];
            } else if (Room.Guests[i].lockedInState == 0)
            {
                guestImageList[i].sprite = GuestImageSprintDictionaries[dictionaryString]["Neutral"];
            } else
            {
                guestImageList[i].sprite = GuestImageSprintDictionaries[dictionaryString]["Put Out"];
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
                foreach (Guest g in Room.Guests)
                {
                    if(g.lockedInState != 1)
                    {
                        g.currentOpinion -= 10;
                    }
                }
                break;
            case 3:
                //3 = Belittle (Sap your Confidence)
                Party.currentPlayerConfidence -= 20;
                break;
            case 4:
                //4 = Antagonize (Uncharm a Charmed Guest, if there is one)
                charmedGuest.currentOpinion = 90;
                charmedGuest.lockedInState = 0;
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
        if(currentTurnTimer/maxTurnTimer >= 0.5 && GameData.playerReputationLevel >= 2)
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
        if(!Room.Guests[guestNumber].dispositionRevealed && Party.currentPlayerIntoxication >= 50)
        {
            return Color.gray;
        }
        if (Party.playerHand[targetingRemark].tone == Room.Guests[guestNumber].disposition.like) //They like the tone
        {
            return Color.green;
        }
        else if (Party.playerHand[targetingRemark].tone == Room.Guests[guestNumber].disposition.dislike) //They dislike the tone
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
        if (Room.Guests[guestNumber].isFemale)
        {
            if (Room.Guests[guestNumber].imageInt == 0)
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
            if (Room.Guests[guestNumber].imageInt == 0)
            {
                dictionaryString = "male0";
            }
            else
            {
                dictionaryString = "male1";
            }
        }
        //Which Guest Image State is Being Selected?
        if (!Room.Guests[guestNumber].dispositionRevealed && Party.currentPlayerIntoxication >= 50) //Is the Player too trashed to notice at all?
        {
            reactionString = "Neutral";
        }
        if (Party.playerHand[targetingRemark].tone == Room.Guests[guestNumber].disposition.like) //They like the tone
        {
            reactionString = "Approve";
        }
        else if (Party.playerHand[targetingRemark].tone == Room.Guests[guestNumber].disposition.dislike) //They dislike the tone
        {
            reactionString = "Disapprove";
        }
        else //Neutral Tone
        {
            reactionString = "Neutral";
        }
        return GuestImageSprintDictionaries[dictionaryString][reactionString];
    }

    Color DispositionImageColor(int guest)
    {
        if (Party.currentPlayerIntoxication >= 50 && Room.Guests[guest].dispositionRevealed == false)
        {
            return Color.gray;
        }
        else
        {
            return GameData.dispositionList[Room.Guests[guest].dispositionInt].color;
        }
    }

    void EndTurn()
    {
        //Reset the Turn Timer
        currentTurnTimer = maxTurnTimer;
        turnTimerBar.value = currentTurnTimer / maxTurnTimer;

        //Increment all the Guest Timers, issue Boredom Damage
        foreach (Guest g in Room.Guests)
        {
            g.currentInterestTimer = Mathf.Clamp(g.currentInterestTimer - 1, 0, g.maxInterestTimer);
            if (g.currentInterestTimer <= 0 && g.lockedInState == 0 && !g.isEnemy) //Guest must not be locked in and must not be an Enemy, Enemies don't get bored 
            {
                g.currentOpinion = Mathf.Clamp(g.currentOpinion - 10, 0, g.maxOpinion);
                if (g.currentOpinion <= 0)
                {
                    g.lockedInState = -1;
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
        femaleGuestImageSpriteDictionary0.Add("Put Out", femaleGuestImage0PutOut);

        femaleGuestImageSpriteDictionary1.Add("Charmed", femaleGuestImage1Charmed);
        femaleGuestImageSpriteDictionary1.Add("Approve", femaleGuestImage1Approve);
        femaleGuestImageSpriteDictionary1.Add("Neutral", femaleGuestImage1Neutral);
        femaleGuestImageSpriteDictionary1.Add("Disapprove", femaleGuestImage1PutOut);
        femaleGuestImageSpriteDictionary1.Add("Put Out", femaleGuestImage1PutOut);

        maleGuestImageSpriteDictionary0.Add("Charmed", maleGuestImage0Charmed);
        maleGuestImageSpriteDictionary0.Add("Approve", maleGuestImage0Approve);
        maleGuestImageSpriteDictionary0.Add("Neutral", maleGuestImage0Neutral);
        maleGuestImageSpriteDictionary0.Add("Disapprove", maleGuestImage0PutOut);
        maleGuestImageSpriteDictionary0.Add("Put Out", maleGuestImage0PutOut);

        maleGuestImageSpriteDictionary1.Add("Charmed", maleGuestImage1Charmed);
        maleGuestImageSpriteDictionary1.Add("Approve", maleGuestImage1Approve);
        maleGuestImageSpriteDictionary1.Add("Neutral", maleGuestImage1Neutral);
        maleGuestImageSpriteDictionary1.Add("Disapprove", maleGuestImage1PutOut);
        maleGuestImageSpriteDictionary1.Add("Put Out", maleGuestImage1PutOut);
    }

    Sprite GenerateGuestImage(int guest)
    {
        if (Room.Guests[guest].isFemale)
        {
            if(Room.Guests[guest].imageInt == 0)
            {
                return femaleGuestImage0Neutral;
            } else
            {
                return femaleGuestImage1Neutral;
            }
        }
        else
        {
            if (Room.Guests[guest].imageInt == 0)
            {
                return maleGuestImage0Neutral;
            }
            else
            {
                return maleGuestImage1Neutral;
            }
        }
    }

    public void SpendConfidenceGetRemark()
    {
        int confidenceCost = 10;
        if (Party.currentPlayerConfidence >= confidenceCost && Party.playerHand.Count < 5)
        {
            Party.currentPlayerConfidence -= confidenceCost;
            AddRemarkToHand();
        }
    }

    public void DrinkForConfidence()
    {
        if (Party.currentPlayerDrinkAmount > 0 && Party.currentPlayerConfidence != Party.maxPlayerConfidence)
        {
            Party.currentPlayerDrinkAmount--;
            Party.currentPlayerConfidence = Mathf.Clamp(Party.currentPlayerConfidence + 20, 20, Party.maxPlayerConfidence);
            int drinkStrength = Party.drinkStrength;
            //Is the Player decent friends with the Military? If so, make them more alcohol tolerant!
            if(GameData.factionList["Military"].ReputationLevel >= 3)
            {
                drinkStrength -= 3;
            }
            //Is the Player using the Snuff Box Accessory? If so, then decrease the Intoxicating Effects of Booze!
            ItemVO accessory;
			if (_inventoryModel.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory.Type == "Snuff Box")
            {
                drinkStrength -= 5;
            }
            Party.currentPlayerIntoxication += drinkStrength;
            foreach (Guest t in Room.Guests)
            {
                t.dispositionRevealed = false;
            }
            if (Party.currentPlayerIntoxication >= Party.maxPlayerIntoxication)
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
		AdjustValueVO vo;
        switch (effectSelection)
        {
            case 1:
                effect = "Reputation Loss";
                effectAmount = Random.Range(20,51) * -1;
                Party.wonRewardsList.Add(new Reward(Party, "Reputation", effectAmount));
                break;
            case 2:
                effect = "Faction Reputation Loss";
                effectAmount = Random.Range(20, 51) * -1;
                Party.wonRewardsList.Add(new Reward(Party, "Faction Reputation", Party.faction, effectAmount));
                break;
            case 3:
                effect = "Outfit Novelty Loss";
                effectAmount = Random.Range(20, 51);
                OutfitInventory.PartyOutfit.novelty = Mathf.Clamp(OutfitInventory.PartyOutfit.novelty - effectAmount, 0, 100);
                break;
            case 4:
                effect = "Outfit Ruined";
                OutfitInventory.personalInventory.Remove(OutfitInventory.PartyOutfit);
                break;
            case 5:
                effect = "Accessory Ruined";
                if(!_inventoryModel.Equipped.Remove(ItemConsts.ACCESSORY)) //If the Player actually wore and Accessory to this Party
                {
                    effect = "Livre Lost";
                    effectAmount = -Random.Range(30, 61);
                    Party.wonRewardsList.Add(new Reward(Party, "Livre", effectAmount));
                    vo = new AdjustValueVO(GameConsts.LIVRE, effectAmount);
                    DeWinterApp.SendMessage<AdjustValueVO>(vo);
                }
                break;
            case 6:
				effect = "Livre Lost";
                effectAmount = -Random.Range(30, 61);
                Party.wonRewardsList.Add(new Reward(Party, "Livre", effectAmount));
                vo = new AdjustValueVO(GameConsts.LIVRE, effectAmount);
                DeWinterApp.SendMessage<AdjustValueVO>(vo);
                break;
            case 7:
                effect = "New Enemy";
                EnemyInventory.AddEnemy(new Enemy(Party.faction));
                break;
            case 8:
                effect = "Forgot All Gossip";
                List<Reward> gossipList = new List<Reward>();
                foreach (Reward r in Party.wonRewardsList)
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
                        Party.wonRewardsList.Remove(r);
                    }
                } else //If they have no Gossip to Lose
                {
                    effect = "New Enemy";
                    EnemyInventory.AddEnemy(new Enemy(Party.faction));
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
                    Party.wonRewardsList.Add(new Reward(Party, "Reputation", effectAmount));
                    break;
                case 2:
                    effect = "Faction Reputation Gain";
                    effectAmount = Random.Range(20, 51);
                    Party.wonRewardsList.Add(new Reward(Party, "Faction Reputation", Party.faction, effectAmount));
                    break;
                case 3:
                    effect = "Livre Gained";
                    effectAmount = Random.Range(30, 61);
                    Party.wonRewardsList.Add(new Reward(Party, "Livre", effectAmount));
                    break;
                case 4:
                    effect = "New Gossip";
                    Party.wonRewardsList.Add(new Reward(Party, "Gossip", 1));
                    break;
                default:
                    effect = "Eliminated an Enemy";
                    if(Party.enemyList.Count == 0)
                    {
                        effect = "New Gossip";
                        Party.wonRewardsList.Add(new Reward(Party, "Gossip", 1));
                    } else
                    {
                        Party.enemyList.RemoveAt(Random.Range(0, Party.enemyList.Count));
                    }
                    break;
            }  
        }
        Party.blackOutEffect = effect;
        Party.blackOutEffectAmount = effectAmount;
        Debug.Log("Black Out Effect Chosen! It's " + effect);
        //Close Window
        Destroy(gameObject);

        //Send to After Party Report Screen
        Debug.Log("Trying to go to the After Party Report Screen!");
        Party.blackOutEnding = true;
        roomManager.partyManager.FinishTheParty();
        levelManager.LoadLevel("Game_AfterPartyReport");
        Debug.Log("At the After Party Report Screen!");
    }

    public Sprite VisualizeTargetingProfile(int remarkInt)
    {
        switch (Party.playerHand[remarkInt].targetingProfileInt)
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
        if (guest.lockedInState == 0) //Is this one locked in yet?
        {
            guest.currentOpinion += amount;
        }
        //Are they Charmed or Put Out?
        if (guest.currentOpinion >= 100 && guest.lockedInState == 0) //If they're not already Charmed then Player Hand is refilled once
        {
            guest.lockedInState = 1;
            RefillPlayerHand();
        }
        else if (guest.currentOpinion <= 0 && guest.lockedInState == 0) //If they're not already Put Out then Player Confidence is reduced by 10
        {
            guest.lockedInState = -1;
            Party.currentPlayerConfidence -= 20;
        }
        if (guest.lockedInState == 1) // If they're Charmed then Opinion is 100
        {
            guest.currentOpinion = 100;
        }
        else if (guest.lockedInState == -1) // If they're Put Out then Opinion is 0
        {
            guest.currentOpinion = 0;
        }
    }

    void AddRemarkToHand()
    {
        if (Party.playerHand.Count < 6) // This is one larger than it should be because remarks are deducted after they're added
        {
            Remark remark = new Remark(Party.lastTone);
            Party.lastTone = remark.tone;
            Party.playerHand.Add(remark);
        }
    }

    void RefillPlayerHand()
    {
        int numberOfCardsForRefill = 5 - Party.playerHand.Count;
        for (int i = 0; i < numberOfCardsForRefill; i++)
        {
            Remark remark = new Remark(Party.lastTone);
            Party.lastTone = remark.tone;
            Party.playerHand.Add(remark);
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
            Party.playerHand.RemoveAt(targetingRemark);
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
            if (guest.lockedInState == 1)
            {
                return "Charmed";
            }
            else if (guest.lockedInState == -1)
            {
                return "Put Out";
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
                if (guest.lockedInState == 1)
                {
                    return "Dazed";
                }
                else if (guest.lockedInState == -1)
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
        //Check to see if everyone is either Charmed or Put Out 
        int charmedAmount = 0;
        int putOutAmount = 0;
        foreach (Guest t in Room.Guests)
        {
            if(t.lockedInState == 1)
            {
                charmedAmount++;
            } else if (t.lockedInState == -1)
            {
                putOutAmount++;
            }
            //If the Conversation is Over
            if (charmedAmount + putOutAmount == Room.Guests.Length)
            {
                Debug.Log("Conversation Over!");
                Room.Cleared = true;
                //Remove the Ambush Cards (If present)
                foreach (Remark r in Party.playerHand)
                {
                    if (r.ambushRemark)
                    {
                        Party.playerHand.Remove(r);
                    }
                }
                //Rewards Distributed Here
                Reward givenReward = Room.Rewards[charmedAmount]; //Amount of Charmed Guests determines the level of Reward.
                if (givenReward.Type() == "Introduction")
                {
                    foreach (Reward r in GameData.tonightsParty.wonRewardsList)
                    {
                        //If that Servant has already been Introduced or if the Reward of their Introduction has already been handed out then change the Reward to Gossip
                        if ((r.SubType() == givenReward.SubType() && r.amount > 0) || DeWinterApp.GetModel<ServantModel>().Introduced.ContainsKey(givenReward.SubType()))
                        {
                            givenReward = new Reward(GameData.tonightsParty, "Gossip", 1);
                        }
                    }
                }
                GameData.tonightsParty.wonRewardsList.Add(givenReward);
                object[] objectStorage = new object[4];
                objectStorage[0] = charmedAmount;
                objectStorage[1] = putOutAmount;
                objectStorage[2] = Room.Features.Contains(PartyConstants.HOST);
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
        if (Party.currentPlayerConfidence <= 0)
        {
            //The Player loses a turn
            Party.turnsLeft--;
            //The Player has their Confidence Reset
            Party.currentPlayerConfidence = Party.startingPlayerConfidence / 2;
            //The Player is relocated to the Entrance
            roomManager.MovePlayerToEntrance();
            //The Player's Reputation is Punished
            int reputationLoss = 25;
            int factionReputationLoss = 50;
            GameData.reputationCount -= reputationLoss;
            GameData.factionList[Party.faction].playerReputation -= factionReputationLoss;
            //Explanation Screen Pop Up goes here
            object[] objectStorage = new object[3];
            objectStorage[0] = Party.faction;
            objectStorage[1] = reputationLoss;
            objectStorage[2] = factionReputationLoss;
            screenFader.gameObject.SendMessage("CreateFailedConfidenceModal", objectStorage);
            //The Player is pulled from the Work the Room session
            Destroy(gameObject);
        }
    }

    void InterestTimersDisplayCheck()
    {
        if (Room.Guests[0].lockedInState != 0 || Room.Guests[0].isEnemy)
        {
            guest0InterestBar.image.color = Color.clear;
            guest0InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest0InterestBar.image.color = Color.white;
            guest0InterestBarBackground.color = Color.white;
        }

        if (Room.Guests[1].lockedInState != 0 || Room.Guests[1].isEnemy)
        {
            guest1InterestBar.image.color = Color.clear;
            guest1InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest1InterestBar.image.color = Color.white;
            guest1InterestBarBackground.color = Color.white;
        }

        if (Room.Guests[2].lockedInState != 0 || Room.Guests[2].isEnemy)
        {
            guest2InterestBar.image.color = Color.clear;
            guest2InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest2InterestBar.image.color = Color.white;
            guest2InterestBarBackground.color = Color.white;
        }

        if (Room.Guests[3].lockedInState != 0 || Room.Guests[3].isEnemy)
        {
            guest3InterestBar.image.color = Color.clear;
            guest3InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest3InterestBar.image.color = Color.white;
            guest3InterestBarBackground.color = Color.white;
        }
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
        Debug.Log("Next Turn Timer Started!");
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
            if (Party.playerHand.ElementAtOrDefault(i) != null)
            {
                if (!Party.playerHand[i].ambushRemark)
                {
                    remarkSlotList[i].targetingProfileImage.color = Color.white;
                    remarkSlotList[i].targetingProfileImage.sprite = VisualizeTargetingProfile(i);
                    remarkSlotList[i].dispositionIcon.color = GameData.dispositionList[Party.playerHand[i].toneInt].color;
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
        //Guest 0
        guest0InterestText.text = InterestState(Room.Guests[0]);
        if (Room.Guests[0].isEnemy)
        {
            guest0InterestText.color = Color.red;
        } else
        {
            guest0InterestText.color = Color.white;
        }
        guest0InterestBar.value = InterestTimer(Room.Guests[0]);
        guest0OpinionBar.value = (float)Room.Guests[0].currentOpinion / 100;
        guest0DispositionIcon.color = DispositionImageColor(0);
        //Guest 1
        guest1InterestText.text = InterestState(Room.Guests[1]);
        if (Room.Guests[1].isEnemy)
        {
            guest1InterestText.color = Color.red;
        }
        else
        {
            guest1InterestText.color = Color.white;
        }
        guest1InterestBar.value = InterestTimer(Room.Guests[1]);
        guest1OpinionBar.value = (float)Room.Guests[1].currentOpinion / 100;
        guest1DispositionIcon.color = DispositionImageColor(1);
        //Guest 2
        guest2InterestText.text = InterestState(Room.Guests[2]);
        if (Room.Guests[2].isEnemy)
        {
            guest2InterestText.color = Color.red;
        }
        else
        {
            guest2InterestText.color = Color.white;
        }
        guest2InterestBar.value = InterestTimer(Room.Guests[2]);
        guest2OpinionBar.value = (float)Room.Guests[2].currentOpinion / 100;
        guest2DispositionIcon.color = DispositionImageColor(2);
        //Guest 3
        guest3InterestText.text = InterestState(Room.Guests[3]);
        if (Room.Guests[3].isEnemy)
        {
            guest3InterestText.color = Color.red;
        }
        else
        {
            guest3InterestText.color = Color.white;
        }
        guest3InterestBar.value = InterestTimer(Room.Guests[3]);
        guest3OpinionBar.value = (float)Room.Guests[3].currentOpinion / 100;
        guest3DispositionIcon.color = DispositionImageColor(3);
    }   
}