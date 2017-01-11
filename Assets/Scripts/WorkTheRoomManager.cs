using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorkTheRoomManager : MonoBehaviour
{
    public SceneFadeInOut screenFader;
    public RoomManager roomManager;
    LevelManager levelManager;

    public Room room;
    Text title;
    public bool isAmbush;

    List<Sprite> femaleGuestSpriteList = new List<Sprite>();
    List<Sprite> maleGuestSpriteList = new List<Sprite>();

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

    List<GameObject> guestVisualList;

    public GameObject guest0Visual;
    Text guest0NameText;
    Image guest0GuestImage;
    Text guest0InterestText;
    Scrollbar guest0InterestBar;
    Image guest0InterestBarBackground;
    Scrollbar guest0OpinionBar;
    Image guest0DispositionIcon;

    public GameObject guest1Visual;
    Text guest1NameText;
    Image guest1GuestImage;
    Text guest1InterestText;
    Scrollbar guest1InterestBar;
    Image guest1InterestBarBackground;
    Scrollbar guest1OpinionBar;
    Image guest1DispositionIcon;

    public GameObject guest2Visual;
    Text guest2NameText;
    Image guest2GuestImage;
    Text guest2InterestText;
    Scrollbar guest2InterestBar;
    Image guest2InterestBarBackground;
    Scrollbar guest2OpinionBar;
    Image guest2DispositionIcon;

    public GameObject guest3Visual;
    Text guest3NameText;
    Image guest3GuestImage;
    Text guest3InterestText;
    Scrollbar guest3InterestBar;
    Image guest3InterestBarBackground;
    Scrollbar guest3OpinionBar;
    Image guest3DispositionIcon;

    public Sprite femaleGuestImage0;
    public Sprite femaleGuestImage1;
    public Sprite femaleGuestImage2;
    public Sprite femaleGuestImage3;

    public Sprite maleGuestImage0;
    public Sprite maleGuestImage1;
    public Sprite maleGuestImage2;
    public Sprite maleGuestImage3;
    public Sprite maleGuestImage4;

    private List<RemarkSlot> remarkSlotList;

    public GameObject remarkSlot0;
    Image remarkSlot0TargetingImage;
    Image remarkSlot0DispositionIcon;

    public GameObject remarkSlot1;
    Image remarkSlot1TargetingImage;
    Image remarkSlot1DispositionIcon;

    public GameObject remarkSlot2;
    Image remarkSlot2TargetingImage;
    Image remarkSlot2DispositionIcon;

    public GameObject remarkSlot3;
    Image remarkSlot3TargetingImage;
    Image remarkSlot3DispositionIcon;

    public GameObject remarkSlot4;
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

    // Use this for initialization
    void Start()
    {
        screenFader = this.transform.parent.GetComponent<SceneFadeInOut>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        title = this.transform.Find("TitleText").GetComponent<Text>();
        title.text = room.name;

        //Visualize the Player
        playerNameText = playerVisual.transform.Find("Name").GetComponent<Text>();
        playerNameText.text = "Lady De Winter";
        playerConfidenceText = playerVisual.transform.Find("ConfidenceCounter").GetComponent<Text>();
        playerConfidenceText.text = "Confidence: " + room.party.currentPlayerConfidence + "/" + room.party.maxPlayerConfidence;
        playerConfidenceBar = playerVisual.transform.Find("ConfidenceBar").GetComponent<Scrollbar>();
        playerConfidenceBar.value = (float)room.party.currentPlayerConfidence / room.party.maxPlayerConfidence;
        playerIntoxicationText = playerVisual.transform.Find("DrinkBoozeButton").Find("IntoxicationCounter").GetComponent<Text>();
        playerIntoxicationText.text = "Intoxication: " + room.party.currentPlayerIntoxication + "/" + room.party.maxPlayerIntoxication;
        drinkBoozeButtonImage = playerVisual.transform.Find("DrinkBoozeButton").GetComponent<Image>();

        //Stock the Guest Images Lists
        StockGuestImageLists();

        //Set Up the Guests
        SetUpGuests();

        //Generate the Remarks
        if (isAmbush)
        {
            if (room.party.playerHand.Count == 5)
            {
                room.party.playerHand.RemoveAt(4);
                room.party.playerHand.RemoveAt(3);
            }
            if (room.party.playerHand.Count == 4)
            {
                room.party.playerHand.RemoveAt(3);
            }
            room.party.playerHand.Add(new Remark("ambush"));
            room.party.playerHand.Add(new Remark("ambush"));
        }

        //Set Up Remarks
        remarkSlotList = new List<RemarkSlot>();
        remarkSlotList.Add(remarkSlot0.GetComponent<RemarkSlot>());
        remarkSlotList.Add(remarkSlot1.GetComponent<RemarkSlot>());
        remarkSlotList.Add(remarkSlot2.GetComponent<RemarkSlot>());
        remarkSlotList.Add(remarkSlot3.GetComponent<RemarkSlot>());
        remarkSlotList.Add(remarkSlot4.GetComponent<RemarkSlot>());

        //Turn Timer
        turnTimerBar.value = currentTurnTimer / maxTurnTimer;

        //Ready Go Text
        readyGoText.text = GameData.conversationIntroList[Random.Range(0, GameData.conversationIntroList.Count)];
        StartCoroutine(ConversationStartTimerWait());

        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
        if (AccessoryInventory.personalInventory[GameData.partyAccessoryID].Type() == "Fascinator")
        {
            fascinatorEffect = true;
        } else
        {
            fascinatorEffect = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Visualize the Player---------------
        playerConfidenceText.text = "Confidence: " + room.party.currentPlayerConfidence + "/" + room.party.maxPlayerConfidence;
        playerConfidenceBar.value = (float)room.party.currentPlayerConfidence / room.party.maxPlayerConfidence;
        if (room.party.currentPlayerConfidence <= 25)
        {
            playerConfidenceText.color = Color.red;
        }
        else
        {
            playerConfidenceText.color = Color.white;
        }
        playerIntoxicationText.text = "Intoxication: " + room.party.currentPlayerIntoxication + "/" + room.party.maxPlayerIntoxication;
        playerDrinkAmountText.text = "Booze Glass: " + room.party.currentPlayerDrinkAmount + "/" + room.party.maxPlayerDrinkAmount;
        if (room.party.currentPlayerDrinkAmount > 0)
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
        guest0NameText = guest0Visual.transform.Find("Name").GetComponent<Text>();
        guest0NameText.text = room.guestList[0].name;
        guest0GuestImage = guest0Visual.transform.Find("GuestImage").GetComponent<Image>();
        guest0GuestImage.sprite = GenerateGuestImage(0);
        guest0InterestText = guest0Visual.transform.Find("InterestText").GetComponent<Text>();
        guest0InterestBar = guest0Visual.transform.Find("InterestBar").GetComponent<Scrollbar>();
        guest0InterestBarBackground = guest0Visual.transform.Find("InterestBar").GetComponent<Image>();
        guest0OpinionBar = guest0Visual.transform.Find("OpinionBar").GetComponent<Scrollbar>();
        guest0DispositionIcon = guest0Visual.transform.Find("DispositionIcon").GetComponent<Image>();
        guest0DispositionIcon.color = DispositionImageColor(0);
        if (room.guestList[0].isEnemy)
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
        guest1NameText = guest1Visual.transform.Find("Name").GetComponent<Text>();
        guest1NameText.text = room.guestList[1].name;
        guest1GuestImage = guest1Visual.transform.Find("GuestImage").GetComponent<Image>();
        guest1GuestImage.sprite = GenerateGuestImage(1);
        guest1InterestText = guest1Visual.transform.Find("InterestText").GetComponent<Text>();
        guest1InterestBar = guest1Visual.transform.Find("InterestBar").GetComponent<Scrollbar>();
        guest1InterestBarBackground = guest1Visual.transform.Find("InterestBar").GetComponent<Image>();
        guest1OpinionBar = guest1Visual.transform.Find("OpinionBar").GetComponent<Scrollbar>();
        guest1DispositionIcon = guest1Visual.transform.Find("DispositionIcon").GetComponent<Image>();
        guest1DispositionIcon.color = DispositionImageColor(1);
        if (room.guestList[1].isEnemy)
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
        guest2NameText = guest2Visual.transform.Find("Name").GetComponent<Text>();
        guest2NameText.text = room.guestList[2].name;
        guest2GuestImage = guest2Visual.transform.Find("GuestImage").GetComponent<Image>();
        guest2GuestImage.sprite = GenerateGuestImage(2);
        guest2InterestText = guest2Visual.transform.Find("InterestText").GetComponent<Text>();
        guest2InterestBar = guest2Visual.transform.Find("InterestBar").GetComponent<Scrollbar>();
        guest2InterestBarBackground = guest2Visual.transform.Find("InterestBar").GetComponent<Image>();
        guest2OpinionBar = guest2Visual.transform.Find("OpinionBar").GetComponent<Scrollbar>();
        guest2DispositionIcon = guest2Visual.transform.Find("DispositionIcon").GetComponent<Image>();
        guest2DispositionIcon.color = DispositionImageColor(2);
        if (room.guestList[2].isEnemy)
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
        guest3NameText = guest3Visual.transform.Find("Name").GetComponent<Text>();
        guest3NameText.text = room.guestList[3].name;
        guest3GuestImage = guest3Visual.transform.Find("GuestImage").GetComponent<Image>();
        guest3GuestImage.sprite = GenerateGuestImage(3);
        guest3InterestText = guest3Visual.transform.Find("InterestText").GetComponent<Text>();
        guest3InterestBar = guest3Visual.transform.Find("InterestBar").GetComponent<Scrollbar>();
        guest3InterestBarBackground = guest3Visual.transform.Find("InterestBar").GetComponent<Image>();
        guest3OpinionBar = guest3Visual.transform.Find("OpinionBar").GetComponent<Scrollbar>();
        guest3DispositionIcon = guest3Visual.transform.Find("DispositionIcon").GetComponent<Image>();
        guest3DispositionIcon.color = DispositionImageColor(3);
        if (room.guestList[3].isEnemy)
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
            if (room.party.playerHand[selectedRemark] != null && !room.party.playerHand[selectedRemark].ambushRemark)
            {
                targetingMode = true;
                targetingRemark = selectedRemark;
                //Debug.Log("Remark Tone: " + GameData.dispositionList[room.party.playerHand[selectedRemark].toneInt].like + " " + room.party.playerHand[selectedRemark].toneInt);
                //Debug.Log("Remark Targeting Profile: " + room.party.playerHand[selectedRemark].targetingProfileInt);
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
            switch (room.party.playerHand[targetingRemark].targetingProfileInt)
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
            room.party.playerHand.RemoveAt(targetingRemark);
            targetingRemark = -1;
            targetingMode = false;
            if (room.party.playerHand.Count == 0)
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
        if (guestNumber >= room.guestList.Count)
        {
            guestNumber -= room.guestList.Count;
        } else if (guestNumber < 0)
        {
            guestNumber += room.guestList.Count;
        }
        Guest guest = room.guestList[guestNumber];
        //Do they like the Tone?
        if (room.party.playerHand[targetingRemark].tone == guest.disposition.like) //They like the tone
        {
            ChangeGuestOpinion(guest, (int)(Random.Range(25,36)));
            AddRemarkToHand(); //Add a new Remark for Tone success           
            room.party.currentPlayerConfidence = Mathf.Clamp(room.party.currentPlayerConfidence + 5, 5, room.party.maxPlayerConfidence); //Confidence Reward
            guest.dispositionRevealed = true; // Reveal their Disposition to the Player (if concealed)
        } else if (room.party.playerHand[targetingRemark].tone == guest.disposition.dislike) //They dislike the tone
        {
            if (!fascinatorEffect) //If the the Player doesn't have the Fascinator Accessory or its ability has already been used up
            {
                ChangeGuestOpinion(guest, (int)(Random.Range(-17, -11)));
                room.party.currentPlayerConfidence = Mathf.Clamp(room.party.currentPlayerConfidence - 10, 0, room.party.maxPlayerConfidence); //Confidence Penalty
            } else //If it hasn't yet, use up the ability and ignore the first Negative Comment Effect
            {
                fascinatorEffect = false;
            }

        } else //Neutral Tone
        {
            ChangeGuestOpinion(guest, (int)(Random.Range(12, 18) * GoingEarlyBonus()));
        }
        // Refill Interest of the Selected
        guest.currentInterestTimer = guest.maxInterestTimer + 1; //Everyone loses one because of the Turn Timer

        //If the Guest is an Enemy
        if (guest.isEnemy)
        {
            //Hammering on Offended Guests gives confidence
            if (guest.lockedInState == -1)
            {
                room.party.currentPlayerConfidence = Mathf.Clamp(room.party.currentPlayerConfidence + 10, 10, (int)(room.party.maxPlayerConfidence * 1.5));
            }
            //Check for Charmed Guests, this is necessary for the Attack Check below
            Guest charmedGuest = null;
            foreach (Guest g in room.guestList)
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
            switch (room.party.playerHand[targetingRemark].targetingProfileInt)
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
        if (guestNumber >= room.guestList.Count)
        {
            guestNumber -= room.guestList.Count;
        }
        else if (guestNumber < 0)
        {
            guestNumber += room.guestList.Count;
        }
        switch (guestNumber)
        {
            case 0:
                guest0GuestImage.color = ReactionColor(0);
                break;
            case 1:
                guest1GuestImage.color = ReactionColor(1);
                break;
            case 2:
                guest2GuestImage.color = ReactionColor(2);
                break;
            case 3:
                guest3GuestImage.color = ReactionColor(3);
                break;
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
                foreach (Guest g in room.guestList)
                {
                    if(g.lockedInState != 1)
                    {
                        g.currentOpinion -= 10;
                    }
                }
                break;
            case 3:
                //3 = Belittle (Sap your Confidence)
                room.party.currentPlayerConfidence -= 20;
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

    public void GuestUnhighlighting()
    {
        foreach (Image t in guestImageList)
        {
            t.color = Color.white;
        }
    }

    public float GoingEarlyBonus()
    {
        if(currentTurnTimer/maxTurnTimer >= 0.5)
        {
            return 1.25f;
        } else
        {
            return 1.0f;
        }
    }

    Color ReactionColor(int guestNumber)
    {
        if(!room.guestList[guestNumber].dispositionRevealed && room.party.currentPlayerIntoxication >= 50)
        {
            return Color.gray;
        }
        if (room.party.playerHand[targetingRemark].tone == room.guestList[guestNumber].disposition.like) //They like the tone
        {
            return Color.green;
        }
        else if (room.party.playerHand[targetingRemark].tone == room.guestList[guestNumber].disposition.dislike) //They dislike the tone
        {
            return Color.red;
        }
        else //Neutral Tone
        {
            return Color.gray;
        }
    }

    Color DispositionImageColor(int guest)
    {
        if (room.party.currentPlayerIntoxication >= 50 && room.guestList[guest].dispositionRevealed == false)
        {
            return Color.gray;
        }
        else
        {
            return GameData.dispositionList[room.guestList[guest].dispositionInt].color;
        }
    }

    void EndTurn()
    {
        //Reset the Turn Timer
        currentTurnTimer = maxTurnTimer;
        turnTimerBar.value = currentTurnTimer / maxTurnTimer;

        //Increment all the Guest Timers, issue Boredom Damage
        foreach (Guest g in room.guestList)
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

    void StockGuestImageLists()
    {
        femaleGuestSpriteList.Add(femaleGuestImage0);
        femaleGuestSpriteList.Add(femaleGuestImage1);
        femaleGuestSpriteList.Add(femaleGuestImage2);
        femaleGuestSpriteList.Add(femaleGuestImage3);

        maleGuestSpriteList.Add(maleGuestImage0);
        maleGuestSpriteList.Add(maleGuestImage1);
        maleGuestSpriteList.Add(maleGuestImage2);
        maleGuestSpriteList.Add(maleGuestImage3);
        maleGuestSpriteList.Add(maleGuestImage4);
    }

    Sprite GenerateGuestImage(int guest)
    {
        if (room.guestList[guest].isFemale)
        {
            return femaleGuestSpriteList[room.guestList[guest].imageInt];
        }
        else
        {
            return maleGuestSpriteList[room.guestList[guest].imageInt];
        }
    }

    public void SpendConfidenceGetRemark()
    {
        int confidenceCost = 10;
        if (room.party.currentPlayerConfidence >= confidenceCost && room.party.playerHand.Count < 5)
        {
            room.party.currentPlayerConfidence -= confidenceCost;
            AddRemarkToHand();
        }
    }

    public void DrinkForConfidence()
    {
        if (room.party.currentPlayerDrinkAmount > 0 && room.party.currentPlayerConfidence != room.party.maxPlayerConfidence)
        {
            room.party.currentPlayerDrinkAmount--;
            room.party.currentPlayerConfidence = Mathf.Clamp(room.party.currentPlayerConfidence + 20, 20, room.party.maxPlayerConfidence);
            int drinkStrength = room.party.drinkStrength;
            //Is the Player using the Snuff Box Accessory? If so then decrease the Intoxicating Effects of Booze!
            if (GameData.partyAccessoryID != -1)
            {
                if (AccessoryInventory.personalInventory[GameData.partyAccessoryID].Type() == "Snuff Box")
                {
                    drinkStrength -= 5;
                }
            }        
            room.party.currentPlayerIntoxication += drinkStrength;
            foreach (Guest t in room.guestList)
            {
                t.dispositionRevealed = false;
            }
            if (room.party.currentPlayerIntoxication >= room.party.maxPlayerIntoxication)
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
                room.party.wonRewardsList.Add(new Reward(room.party, "Reputation", effectAmount));
                break;
            case 2:
                effect = "Faction Reputation Loss";
                effectAmount = Random.Range(20, 51) * -1;
                room.party.wonRewardsList.Add(new Reward(room.party, "Faction Reputation", room.party.faction, effectAmount));
                break;
            case 3:
                effect = "Outfit Novelty Loss";
                effectAmount = Random.Range(20, 51);
                OutfitInventory.personalInventory[GameData.partyOutfitID].novelty = Mathf.Clamp(OutfitInventory.personalInventory[GameData.partyOutfitID].novelty - effectAmount, 0, 100);
                break;
            case 4:
                effect = "Outfit Ruined";
                OutfitInventory.personalInventory.RemoveAt(GameData.partyOutfitID);
                break;
            case 5:
                effect = "Accessory Ruined";
                if(GameData.partyAccessoryID != -1) //If the Player actually wore and Accessory to this Party
                {
                    AccessoryInventory.personalInventory.RemoveAt(GameData.partyAccessoryID);
                } else
                {
                    effect = "Livre Lost";
                    effectAmount = Random.Range(30, 61) * -1;
                    room.party.wonRewardsList.Add(new Reward(room.party, "Livre", effectAmount));
                }
                break;
            case 6:
                effect = "Livre Lost";
                effectAmount = Random.Range(30, 61) * -1;
                room.party.wonRewardsList.Add(new Reward(room.party, "Livre", effectAmount));
                break;
            case 7:
                effect = "New Enemy";
                EnemyInventory.AddEnemy(new Enemy(GameData.factionList[room.party.faction]));
                break;
            case 8:
                effect = "Forgot All Gossip";
                List<Reward> gossipList = new List<Reward>();
                foreach (Reward r in room.party.wonRewardsList)
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
                        room.party.wonRewardsList.Remove(r);
                    }
                } else //If they have no Gossip to Lose
                {
                    effect = "New Enemy";
                    EnemyInventory.AddEnemy(new Enemy(GameData.factionList[room.party.faction]));
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
                    room.party.wonRewardsList.Add(new Reward(room.party, "Reputation", effectAmount));
                    break;
                case 2:
                    effect = "Faction Reputation Gain";
                    effectAmount = Random.Range(20, 51);
                    room.party.wonRewardsList.Add(new Reward(room.party, "Faction Reputation", room.party.faction, effectAmount));
                    break;
                case 3:
                    effect = "Livre Gained";
                    effectAmount = Random.Range(30, 61);
                    room.party.wonRewardsList.Add(new Reward(room.party, "Livre", effectAmount));
                    break;
                case 4:
                    effect = "New Gossip";
                    room.party.wonRewardsList.Add(new Reward(room.party, "Gossip", 1));
                    break;
                default:
                    effect = "Eliminated an Enemy";
                    if(room.party.enemyList.Count == 0)
                    {
                        effect = "New Gossip";
                        room.party.wonRewardsList.Add(new Reward(room.party, "Gossip", 1));
                    } else
                    {
                        room.party.enemyList.RemoveAt(Random.Range(0, room.party.enemyList.Count));
                    }
                    break;
            }  
        }
        room.party.blackOutEffect = effect;
        room.party.blackOutEffectAmount = effectAmount;
        Debug.Log("Black Out Effect Chosen! It's " + effect);
        //Close Window
        Destroy(gameObject);
        GameData.activeModals--;

        //Send to After Party Report Screen
        Debug.Log("Trying to go to the After Party Report Screen!");
        room.party.blackOutEnding = true;
        roomManager.partyManager.FinishTheParty();
        levelManager.LoadLevel("Game_AfterPartyReport");
        Debug.Log("At the After Party Report Screen!");
    }

    public Sprite VisualizeTargetingProfile(int remarkInt)
    {
        switch (room.party.playerHand[remarkInt].targetingProfileInt)
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
            room.party.currentPlayerConfidence -= 20;
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
        if (room.party.playerHand.Count < 6) // This is one larger than it should be because remarks are deducted after they're added
        {
            Remark remark = new Remark(room.party.lastTone);
            room.party.lastTone = remark.tone;
            room.party.playerHand.Add(remark);
        }
    }

    void RefillPlayerHand()
    {
        int numberOfCardsForRefill = 5 - room.party.playerHand.Count;
        for (int i = 0; i < numberOfCardsForRefill; i++)
        {
            Remark remark = new Remark(room.party.lastTone);
            room.party.lastTone = remark.tone;
            room.party.playerHand.Add(remark);
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
            room.party.playerHand.RemoveAt(targetingRemark);
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
        foreach (Guest t in room.guestList)
        {
            if(t.lockedInState == 1)
            {
                charmedAmount++;
            } else if (t.lockedInState == -1)
            {
                putOutAmount++;
            }
            //If the Conversation is Over
            if (charmedAmount + putOutAmount == room.guestList.Count)
            {
                Debug.Log("Conversation Over!");
                room.cleared = true;
                //Remove the Ambush Cards (If present)
                foreach (Remark r in room.party.playerHand)
                {
                    if (r.ambushRemark)
                    {
                        room.party.playerHand.Remove(r);
                    }
                }
                //Rewards Distributed Here
                Reward givenReward = room.rewardList[charmedAmount]; //Amount of Charmed Guests determines the level of Reward.
                if (givenReward.Type() == "Introduction")
                {
                    foreach (Reward r in GameData.tonightsParty.wonRewardsList)
                    {
                        //If that Servant has already been Introduced or if the Reward of their Introduction has already been handed out then change the Reward to Gossip
                        if ((r.SubType() == givenReward.SubType() && r.amount > 0) || GameData.servantDictionary[givenReward.SubType()].Introduced())
                        {
                            givenReward = new Reward(GameData.tonightsParty, "Gossip", 1);
                        }
                    }
                }
                GameData.tonightsParty.wonRewardsList.Add(givenReward);
                object[] objectStorage = new object[4];
                objectStorage[0] = charmedAmount;
                objectStorage[1] = putOutAmount;
                objectStorage[2] = room.hostHere;
                objectStorage[3] = givenReward;
                screenFader.gameObject.SendMessage("WorkTheRoomReportModal", objectStorage);
                //Close the Window
                Destroy(gameObject);
                GameData.activeModals--;
            }
        }
    }

    void ConfidenceCheck()
    {
        //Check to see if the Player has run out of Confidence
        if (room.party.currentPlayerConfidence <= 0)
        {
            //The Player loses a turn
            room.party.turnsLeft--;
            //The Player has their Confidence Reset
            room.party.currentPlayerConfidence = room.party.startingPlayerConfidence / 2;
            //The Player is relocated to the Entrance
            roomManager.MovePlayerToEntrance();
            //The Player's Reputation is Punished
            int reputationLoss = 25;
            int factionReputationLoss = 50;
            GameData.reputationCount -= reputationLoss;
            GameData.factionList[room.party.faction].playerReputation -= factionReputationLoss;
            //Explanation Screen Pop Up goes here
            object[] objectStorage = new object[3];
            objectStorage[0] = room.party.faction;
            objectStorage[1] = reputationLoss;
            objectStorage[2] = factionReputationLoss;
            screenFader.gameObject.SendMessage("CreateFailedConfidenceModal", objectStorage);
            //The Player is pulled from the Work the Room session
            Destroy(gameObject);
            GameData.activeModals--;
        }
    }

    void InterestTimersDisplayCheck()
    {
        if (room.guestList[0].lockedInState != 0 || room.guestList[0].isEnemy)
        {
            guest0InterestBar.image.color = Color.clear;
            guest0InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest0InterestBar.image.color = Color.white;
            guest0InterestBarBackground.color = Color.white;
        }

        if (room.guestList[1].lockedInState != 0 || room.guestList[1].isEnemy)
        {
            guest1InterestBar.image.color = Color.clear;
            guest1InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest1InterestBar.image.color = Color.white;
            guest1InterestBarBackground.color = Color.white;
        }

        if (room.guestList[2].lockedInState != 0 || room.guestList[2].isEnemy)
        {
            guest2InterestBar.image.color = Color.clear;
            guest2InterestBarBackground.color = Color.clear;
        }
        else
        {
            guest2InterestBar.image.color = Color.white;
            guest2InterestBarBackground.color = Color.white;
        }

        if (room.guestList[3].lockedInState != 0 || room.guestList[3].isEnemy)
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
            if (room.party.playerHand.ElementAtOrDefault(i) != null)
            {
                if (!room.party.playerHand[i].ambushRemark)
                {
                    remarkSlotList[i].targetingProfileImage.color = Color.white;
                    remarkSlotList[i].targetingProfileImage.sprite = VisualizeTargetingProfile(i);
                    remarkSlotList[i].dispositionIcon.color = GameData.dispositionList[room.party.playerHand[i].toneInt].color;
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
        guest0InterestText.text = InterestState(room.guestList[0]);
        if (room.guestList[0].isEnemy)
        {
            guest0InterestText.color = Color.red;
        } else
        {
            guest0InterestText.color = Color.white;
        }
        guest0InterestBar.value = InterestTimer(room.guestList[0]);
        guest0OpinionBar.value = (float)room.guestList[0].currentOpinion / 100;
        guest0DispositionIcon.color = DispositionImageColor(0);
        guest1InterestText.text = InterestState(room.guestList[1]);
        if (room.guestList[1].isEnemy)
        {
            guest1InterestText.color = Color.red;
        }
        else
        {
            guest1InterestText.color = Color.white;
        }
        guest1InterestBar.value = InterestTimer(room.guestList[1]);
        guest1OpinionBar.value = (float)room.guestList[1].currentOpinion / 100;
        guest1DispositionIcon.color = DispositionImageColor(1);
        guest2InterestText.text = InterestState(room.guestList[2]);
        if (room.guestList[2].isEnemy)
        {
            guest2InterestText.color = Color.red;
        }
        else
        {
            guest2InterestText.color = Color.white;
        }
        guest2InterestBar.value = InterestTimer(room.guestList[2]);
        guest2OpinionBar.value = (float)room.guestList[2].currentOpinion / 100;
        guest2DispositionIcon.color = DispositionImageColor(2);
        guest3InterestText.text = InterestState(room.guestList[3]);
        if (room.guestList[3].isEnemy)
        {
            guest3InterestText.color = Color.red;
        }
        else
        {
            guest3InterestText.color = Color.white;
        }
        guest3InterestBar.value = InterestTimer(room.guestList[3]);
        guest3OpinionBar.value = (float)room.guestList[3].currentOpinion / 100;
        guest3DispositionIcon.color = DispositionImageColor(3);
    }
}

    
