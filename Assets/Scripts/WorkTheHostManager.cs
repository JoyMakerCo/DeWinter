using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DeWinter;

public class WorkTheHostManager : MonoBehaviour
{
    public SceneFadeInOut screenFader;
    public RoomManager roomManager;
    LevelManager levelManager;

    public RoomVO room;
    Text title;
    public bool isAmbush;

    List<Sprite> femaleHostSpriteList = new List<Sprite>();
    List<Sprite> maleHostSpriteList = new List<Sprite>();

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

    public GameObject hostVisual;
    Text hostNameText;
    Image hostImage;
    public Text hostInterestText;
    Scrollbar hostInterestBar;
    Image hostInterestBarBackground;
    Scrollbar hostOpinionBar;
    Image hostDispositionIcon;

    float hostDispositionSwitchTimer;

    public Sprite femaleHostImage0;
    public Sprite femaleHostImage1;
    public Sprite femaleHostImage2;
    public Sprite femaleHostImage3;

    public Sprite maleHostImage0;
    public Sprite maleHostImage1;
    public Sprite maleHostImage2;
    public Sprite maleHostImage3;
    public Sprite maleHostImage4;

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

    public bool targetingMode = false; //Are we selecting an target for a remark right now?
    int targetingRemark = -1;
    bool targetingFlipped = false; //Has targetting been flipped? Necessary for the Targeting Profiles

    bool conversationStarted = false;
    public Image readyGoPanel;
    public Text readyGoText;

    public GameObject hostRemarkWindow;
    public float hostRemarkTimer; // This determines how long it is between instances of Host Remarks being fired back
    public bool hostRemarkActive = false;
    public bool hostRemarkStarted = false;
    float hostRemarkActiveTimer; //This determines how long the Player has to finish up a particular Host Remark
    public List<FireBackRemarkSlot> hostRemarkSlotList = new List<FireBackRemarkSlot>();
    public List<FireBackRemarkSlotButton> hostRemarkSlotButtonList = new List<FireBackRemarkSlotButton>();
    public int hostRemarkSlotComplete = 0; //Used to determine if all the FireBackRemarkSlots have been finished or not
    public Scrollbar hostRemarkCountdownBar;

    private PartyModel _partyModel;
    private MapModel _mapModel;

    public Party Party
    {
		get { return _partyModel.Party; }
    }

    // Use this for initialization
    void Start()
    {
    	_partyModel = DeWinterApp.GetModel<PartyModel>();
		_mapModel = DeWinterApp.GetModel<MapModel>();
    	
        screenFader = this.transform.parent.GetComponent<SceneFadeInOut>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        title = this.transform.Find("TitleText").GetComponent<Text>();
        title.text = room.Name;

        //Set Up the Player
        playerNameText = playerVisual.transform.Find("Name").GetComponent<Text>();
        playerNameText.text = "Lady De Winter";
        playerConfidenceText = playerVisual.transform.Find("ConfidenceCounter").GetComponent<Text>();
        playerConfidenceText.text = "Confidence: " + Party.currentPlayerConfidence + "/" + Party.maxPlayerConfidence;
        playerConfidenceBar = playerVisual.transform.Find("ConfidenceBar").GetComponent<Scrollbar>();
        playerConfidenceBar.value = (float)Party.currentPlayerConfidence / Party.maxPlayerConfidence;
        playerIntoxicationText = playerVisual.transform.Find("DrinkBoozeButton").Find("IntoxicationCounter").GetComponent<Text>();
        playerIntoxicationText.text = "Intoxication: " + Party.currentPlayerIntoxication + "/" + Party.maxPlayerIntoxication;
        drinkBoozeButtonImage = playerVisual.transform.Find("DrinkBoozeButton").GetComponent<Image>();

        //Stock the Host Images Lists
        StockHostImageLists();

        //Set Up The Host
        hostNameText = hostVisual.transform.Find("Name").GetComponent<Text>();
        hostNameText.text = Party.host.name;
        hostImage = hostVisual.transform.Find("HostImage").GetComponent<Image>();
        hostImage.sprite = GenerateHostImage();
        hostInterestBar = hostVisual.transform.Find("InterestBar").GetComponent<Scrollbar>();
        hostInterestBarBackground = hostVisual.transform.Find("InterestBar").GetComponent<Image>();
        hostOpinionBar = hostVisual.transform.Find("OpinionBar").GetComponent<Scrollbar>();
        hostDispositionIcon = hostVisual.transform.Find("DispositionIcon").GetComponent<Image>();
        hostDispositionIcon.color = HostDispositionImageColor();

        //Generate the Remarks, it's not possible to be Ambushed by the Host
        if (isAmbush)
        {
            if (Party.playerHand.Count == 5)
            {
                Party.playerHand.RemoveAt(4);
            }
            Party.playerHand.Add(new Remark("ambush"));
        }

        //Set Up the Remarks--------------------
        remarkSlotList = new List<RemarkSlot>();
        remarkSlotList.Add(remarkSlot0.GetComponent<RemarkSlot>());
        remarkSlotList.Add(remarkSlot1.GetComponent<RemarkSlot>());
        remarkSlotList.Add(remarkSlot2.GetComponent<RemarkSlot>());
        remarkSlotList.Add(remarkSlot3.GetComponent<RemarkSlot>());
        remarkSlotList.Add(remarkSlot4.GetComponent<RemarkSlot>());

        hostDispositionSwitchTimer = Party.host.dispositionTimerSwitchMax;
        hostRemarkTimer = Party.host.nextHostRemarkTimer;

        //Is the Player using the Fan Accessory? If so then increase the Host Remark Time by 1.5x!
        if(Party.playerAccessory != null)
        {
            if (Party.playerAccessory.Type() == "Fan")
            {
                Party.host.currentInterestTimer = Party.host.currentInterestTimer * 1.5f;
                Party.host.maxInterestTimer = Party.host.currentInterestTimer * 1.5f;

            }
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
        playerDrinkAmountText.text = "Booze Glass: " + Party.currentPlayerDrinkAmount + "/" + Party.maxPlayerDrinkAmount;
        if (Party.currentPlayerDrinkAmount > 0)
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
        hostOpinionBar.value = (float)Party.host.currentOpinion / Party.host.maxOpinion;
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
            Party.host.hostRemarkCompletionTimerCurrent -= Time.deltaTime;
            hostRemarkCountdownBar.value = Party.host.hostRemarkCompletionTimerCurrent / Party.host.hostRemarkCompletionTimerMax;
            //Did the Player not clear it in time?
            if (Party.host.hostRemarkCompletionTimerCurrent < 0)
            {
                Party.host.currentOpinion -= 20;
                Party.currentPlayerConfidence -= 25;
                //Kill the Window
                Destroy(hostRemarkWindow);
                Party.host.hostRemarkCompletionTimerCurrent = Party.host.hostRemarkCompletionTimerMax;
                hostRemarkActive = false;
                Debug.Log("Host Remark Failed! :-(");
            }
            //Did the Player clear it in time?
            if (hostRemarkSlotComplete >= hostRemarkSlotList.Count)
            {
                //Kill the Window
                Destroy(hostRemarkWindow);
                Party.host.hostRemarkCompletionTimerCurrent = Party.host.hostRemarkCompletionTimerMax;
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

    public void StartTargeting(int selectedRemark)
    {
        if (conversationStarted || hostRemarkActive)
        {
            if (Party.playerHand[selectedRemark] != null && !Party.playerHand[selectedRemark].ambushRemark)
            {
                targetingMode = true;
                targetingRemark = selectedRemark;
                Debug.Log("Remark Tone: " + GameData.dispositionList[Party.playerHand[selectedRemark].toneInt].like + " " + Party.playerHand[selectedRemark].toneInt);
            }
            else
            {
                Debug.Log("No Remark to Select");
            }
        }
    }

    public void HostSelected()
    {
        if (targetingMode) //If a remark has been selected
        {
            HostTargeted();
            //Deselect any remarks
            Party.playerHand.RemoveAt(targetingRemark);
            targetingRemark = -1;
            targetingMode = false;
            if (Party.playerHand.Count == 0)
            {
                SpendConfidenceGetRemark();
            }
            EndTurn();
        }
        else
        {
            Debug.Log("No remark selected :(");
        }   
    }

    void HostTargeted()
    {
        //Do they like the Tone?
        if (Party.playerHand[targetingRemark].tone == Party.host.disposition.like) //They like the tone
        {
            ChangeHostOpinion(Random.Range(25, 36));
            AddRemarkToHand(); //Add a new Remark for Tone success           
            Party.currentPlayerConfidence = Mathf.Clamp(Party.currentPlayerConfidence + 5, 5, Party.maxPlayerConfidence); //Confidence Reward
            Party.host.dispositionRevealed = true; // Reveal their Disposition to the Player (if concealed)
        }
        else if (Party.playerHand[targetingRemark].tone == Party.host.disposition.dislike) //They dislike the tone
        {
            ChangeHostOpinion(Random.Range(-17, -11));
            Party.currentPlayerConfidence = Mathf.Clamp(Party.currentPlayerConfidence - 10, 0, Party.maxPlayerConfidence); //Confidence Penalty
        }
        else //Neutral Tone
        {
            ChangeHostOpinion(Random.Range(12, 18));
        }
        // Refill Interest of the Selected
        Party.host.currentInterestTimer = Party.host.maxInterestTimer + 1; //End Turn already Knocks one off so this compensates
        Party.host.interestTimerWaiting = true;
        StartCoroutine(Party.host.TimerWait());
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
            switch (Party.playerHand[targetingRemark].targetingProfileInt)
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
        Party.playerHand.RemoveAt(targetingRemark);
        targetingRemark = -1;
        targetingMode = false;
        if (Party.playerHand.Count == 0)
        {
            SpendConfidenceGetRemark();
        }
    }

    void HostRemarkTargeted(int slotNumber)
    {
        //Wraparound is handled here
        if (slotNumber >= hostRemarkSlotList.Count)
        {
            slotNumber -= hostRemarkSlotList.Count;
        }
        else if (slotNumber < 0)
        {
            slotNumber += hostRemarkSlotList.Count;
        }
        //Do they like the Tone?
        if (Party.playerHand[targetingRemark].tone == hostRemarkSlotList[slotNumber].disposition.like) //They like the tone
        {
            if (hostRemarkSlotList[slotNumber].lockedInState != 1)
            {
                hostRemarkSlotComplete++;
            }
            hostRemarkSlotList[slotNumber].lockedInState = 1;
            AddRemarkToHand(); //Add a new Remark for Tone success           
            Party.currentPlayerConfidence = Mathf.Clamp(Party.currentPlayerConfidence + 5, 5, Party.maxPlayerConfidence); //Confidence Reward        
        }
        else if (Party.playerHand[targetingRemark].tone == hostRemarkSlotList[slotNumber].disposition.dislike) //They dislike the tone
        {
            Party.currentPlayerConfidence = Mathf.Clamp(Party.currentPlayerConfidence - 10, 0, Party.maxPlayerConfidence); //Confidence Penalty
        }
        else //Neutral Tone
        {
            hostRemarkSlotComplete++;
            hostRemarkSlotList[slotNumber].lockedInState = 1;
        }
        // Up the Remaining Timer?
        Party.host.hostRemarkCompletionTimerCurrent = Mathf.Clamp(Party.host.hostRemarkCompletionTimerCurrent + 1.0f, 1.0f, Party.host.hostRemarkCompletionTimerMax);
    }

    public void HostRemarkSlotHighlighting(int slotNumber)
    {
        if (targetingMode)
        {
            switch (Party.playerHand[targetingRemark].targetingProfileInt)
            {
                case 1:
                    //No flip necessary, it's just one target
                        HostRemarkSlotHighlight(slotNumber);
                    break;
                case 11:
                    //Flip versions
                    if (!targetingFlipped)
                    {
                        HostRemarkSlotHighlight(slotNumber);
                        HostRemarkSlotHighlight(slotNumber + 1);
                    }
                    else
                    {
                        HostRemarkSlotHighlight(slotNumber);
                        HostRemarkSlotHighlight(slotNumber - 1);
                    }
                    break;
                case 101:
                    //Flip Versions
                    if (!targetingFlipped)
                    {
                        HostRemarkSlotHighlight(slotNumber);
                        HostRemarkSlotHighlight(slotNumber + 2);
                    }
                    else
                    {
                        HostRemarkSlotHighlight(slotNumber);
                        HostRemarkSlotHighlight(slotNumber - 2);
                    }
                    break;
                case 1011:
                    //Flip version
                    if (!targetingFlipped)
                    {
                        HostRemarkSlotHighlight(slotNumber);
                        HostRemarkSlotHighlight(slotNumber + 2);
                        HostRemarkSlotHighlight(slotNumber + 3);
                    }
                    else
                    {
                        HostRemarkSlotHighlight(slotNumber);
                        HostRemarkSlotHighlight(slotNumber - 2);
                        HostRemarkSlotHighlight(slotNumber - 3);
                    }
                    break;
            }
        }
    }

    void HostRemarkSlotHighlight(int slotNumber)
    {
        if (slotNumber >= hostRemarkSlotList.Count)
        {
            slotNumber -= hostRemarkSlotList.Count;
        }
        else if (slotNumber < 0)
        {
            slotNumber += hostRemarkSlotList.Count;
        }
        switch (slotNumber)
        {
            case 0:
                hostRemarkSlotButtonList[0].remarkSlotImage.color = HostRemarkSlotReactionColor(0);
                break;
            case 1:
                hostRemarkSlotButtonList[1].remarkSlotImage.color = HostRemarkSlotReactionColor(1);
                break;
            case 2:
                hostRemarkSlotButtonList[2].remarkSlotImage.color = HostRemarkSlotReactionColor(2);
                break;
            case 3:
                hostRemarkSlotButtonList[3].remarkSlotImage.color = HostRemarkSlotReactionColor(3);
                break;
            case 4:
                hostRemarkSlotButtonList[4].remarkSlotImage.color = HostRemarkSlotReactionColor(4);
                break;
        }
    }

    public Color ReactionColor()
    {
        if (!Party.host.dispositionRevealed && Party.currentPlayerIntoxication >= 50)
        {
            return Color.gray;
        }
        if (Party.playerHand[targetingRemark].tone == Party.host.disposition.like) //They like the tone
        {
            return Color.green;
        }
        else if (Party.playerHand[targetingRemark].tone == Party.host.disposition.dislike) //They dislike the tone
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
        if (Party.playerHand[targetingRemark].tone == hostRemarkSlotList[slotNumber].disposition.like) //They like the tone
        {
            return Color.green;
        }
        else if (Party.playerHand[targetingRemark].tone == hostRemarkSlotList[slotNumber].disposition.dislike) //They dislike the tone
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
        if (Party.currentPlayerIntoxication >= 50 && Party.host.dispositionRevealed == false)
        {
            return Color.gray;
        }
        else
        {
            return GameData.dispositionList[Party.host.dispositionInt].color;
        }
    }

    void EndTurn()
    {
        //Reset the Turn Timer
        currentTurnTimer = maxTurnTimer;
        turnTimerBar.value = currentTurnTimer / maxTurnTimer;

        //Increment the Host's Interest Timer, issue Boredom Damage
        Party.host.currentInterestTimer = Mathf.Clamp(Party.host.currentInterestTimer - 1, 0, Party.host.maxInterestTimer);
        if (Party.host.currentInterestTimer <= 0 && Party.host.lockedInState == 0)
        { 
            Party.host.currentOpinion = Mathf.Clamp(Party.host.currentOpinion - 10, 0, Party.host.maxOpinion);
            if (Party.host.currentOpinion <= 0)
            { 
                Party.host.lockedInState = -1;
            }
        }

        //Pause the Next Turn Timer
        turnTimerActive = false;
    }

    void StockHostImageLists()
    {
        femaleHostSpriteList.Add(femaleHostImage0);
        femaleHostSpriteList.Add(femaleHostImage1);
        femaleHostSpriteList.Add(femaleHostImage2);
        femaleHostSpriteList.Add(femaleHostImage3);

        maleHostSpriteList.Add(maleHostImage0);
        maleHostSpriteList.Add(maleHostImage1);
        maleHostSpriteList.Add(maleHostImage2);
        maleHostSpriteList.Add(maleHostImage3);
        maleHostSpriteList.Add(maleHostImage4);
    }

    Sprite GenerateHostImage()
    {
        if (Party.host.isFemale)
        {
            return femaleHostSpriteList[Party.host.imageInt];
        }
        else
        {
            return maleHostSpriteList[Party.host.imageInt];
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
            //Is the Player using the Snuff Box Accessory? If so then decrease the Intoxicating Effects of Booze!
            if (GameData.partyAccessory != null)
            {
                if (GameData.partyAccessory.Type == "Snuff Box")
                {
                    drinkStrength -= 5;
                }
            }
            Party.currentPlayerIntoxication += drinkStrength;
            Party.host.dispositionRevealed = false;
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
        switch (effectSelection)
        {
            case 1:
                effect = "Reputation Loss";
                effectAmount = Random.Range(20, 51) * -1;
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
                OutfitInventory.personalInventory[OutfitInventory.PartyOutfit].novelty = Mathf.Clamp(OutfitInventory.personalInventory[OutfitInventory.PartyOutfit].novelty - effectAmount, 0, 100);
                break;
            case 4:
                effect = "Outfit Ruined";
                OutfitInventory.personalInventory.RemoveAt(OutfitInventory.PartyOutfit);
                break;
            case 5:
                effect = "Accessory Ruined";
                if (GameData.PartyAccessory != -1) //If the Player actually wore and Accessory to this Party
                {
                    GameData.Accessories.RemoveAt(GameData.PartyAccessory);
                }
                else
                {
                    effect = "Livre Lost";
                    effectAmount = Random.Range(30, 61) * -1;
                    Party.wonRewardsList.Add(new Reward(Party, "Livre", effectAmount));
                }
                break;
            case 6:
                effect = "Livre Lost";
                effectAmount = Random.Range(30, 61) * -1;
                Party.wonRewardsList.Add(new Reward(Party, "Livre", effectAmount));
                break;
            case 7:
                effect = "New Enemy";
                EnemyInventory.AddEnemy(new Enemy(GameData.factionList[Party.faction]));
                break;
            case 8:
                effect = "Forgot All Gossip";
                List<Reward> gossipList = new List<Reward>();
                foreach (Reward r in Party.wonRewardsList)
                {
                    if (r.Type() == "Gossip")
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
                }
                else //If they have no Gossip to Lose
                {
                    effect = "New Enemy";
                    EnemyInventory.AddEnemy(new Enemy(GameData.factionList[Party.faction]));
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
                    if (Party.enemyList.Count == 0)
                    {
                        effect = "New Gossip";
                        Party.wonRewardsList.Add(new Reward(Party, "Gossip", 1));
                    }
                    else
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

    void ChangeHostOpinion(int amount)
    {
        if (Party.host.lockedInState == 0) //Is this one locked in yet?
        {
            Party.host.currentOpinion += amount;
        }
        //Are they Charmed or Put Out?
        if (Party.host.currentOpinion >= Party.host.maxOpinion && Party.host.lockedInState == 0) //If they're not already Charmed then Player Hand is refilled once
        {
            Party.host.lockedInState = 1;
            RefillPlayerHand();
        }
        else if (Party.host.currentOpinion <= 0 && Party.host.lockedInState == 0) //If they're not already Put Out then Player Confidence is reduced by 10
        {
            Party.host.lockedInState = -1;
            Party.currentPlayerConfidence -= 10;
        }

        if (Party.host.lockedInState == 1) // If they're Charmed then Opinion is Maxed out
        {
            Party.host.currentOpinion = Party.host.maxOpinion;
        }
        else if (Party.host.lockedInState == -1) // If they're Put Out then Opinion is 0
        {
            Party.host.currentOpinion = 0;
        }
    }

    void AddRemarkToHand()
    {
        if (Party.playerHand.Count < 6) // This is one larger than it should be because Remarks are deducted after they're added
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

    float InterestTimer()
    {
        return Party.host.currentInterestTimer / Party.host.maxInterestTimer;
    }

    string InterestState()
    {
        if (Party.host.lockedInState == 1)
        {
            return "Charmed";
        }
        else if (Party.host.lockedInState == -1)
        {
            return "Put Out";
        }
        else if (Party.host.currentInterestTimer == 0)
        {
            return "BORED!";
        }
        else
        {
            return "Interest:";
        }
    }

    void VictoryCheck()
    {
        //Check to see if everyone is either Charmed or Put Out 
        int charmedAmount = 0;
        int putOutAmount = 0;
        if (Party.host.lockedInState == 1)
        {
            charmedAmount++;
        }
        else if (Party.host.lockedInState == -1)
        {
            putOutAmount++;
        }
        //If the Conversation is Over
        if (charmedAmount + putOutAmount > 0)
        {
            Debug.Log("Conversation Over!");
            room.cleared = true;
            //Rewards and Gossip Distributed Here
            Reward givenReward = room.rewardList[5];  //Hosts give level 5 Rewards
            GameData.tonightsParty.wonRewardsList.Add(givenReward);
            object[] objectStorage = new object[4];
            objectStorage[0] = charmedAmount;
            objectStorage[1] = putOutAmount;
            objectStorage[2] = Party.hostHere;
            objectStorage[3] = givenReward;
            screenFader.gameObject.SendMessage("WorkTheRoomReportModal", objectStorage);
            //Close the Window
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
        if (Party.host.lockedInState != 0)
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

    void HostDispositionSwitchCheck()
    {
        if (hostDispositionSwitchTimer <= 0)
        {
            //Actually Changes the disposition
            Party.host.ChangeDisposition();
            hostDispositionSwitchTimer = Party.host.dispositionTimerSwitchMax;
            hostDispositionIcon.color = GameData.dispositionList[Party.host.dispositionInt].color;
            Debug.Log("Host Disposition Switched!");
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
                objectStorage[0] = Party.host;
                objectStorage[1] = this;
                objectStorage[2] = Random.Range(1, 6);
                screenFader.gameObject.SendMessage("CreateHostRemarkModal", objectStorage);
                Text introText = hostRemarkWindow.transform.FindChild("IntroTextPanel").Find("Text").GetComponent<Text>();
                introText.text = GameData.hostRemarkIntroList[Random.Range(0, GameData.hostRemarkIntroList.Count)];
                //So there'll be a wait before the next Host Remark
                hostRemarkTimer = Party.host.nextHostRemarkTimer;
                //Kills the Wait Screen
                StartCoroutine(HostRemarkTimerWait());
            }
        }      
    }

    public void StockFireBackRemarkSlotList(int slotCount)
    {
        hostRemarkSlotComplete = 0;
        hostRemarkSlotList.Clear();
        for(int i = 0; i < slotCount; i++)
        {
            hostRemarkSlotList.Add(new FireBackRemarkSlot());
        }
    }

    public IEnumerator HostRemarkTimerWait()
    {
        Debug.Log("'But I have a Question' Timer Started");
        yield return new WaitForSeconds(1.0f);
        GameObject introTextPanel = hostRemarkWindow.transform.FindChild("IntroTextPanel").gameObject;
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
}