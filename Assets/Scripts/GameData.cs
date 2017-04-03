using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    static GameData instance = null;

    //Basic Data Book Keeping
    public static string playerCharacter;
    public static string gameDifficulty;
    public static bool fTEsOn;

    public static int moneyCount;
    public static int currentMonth;
    public static int currentDay;
    public static int startMonthInt;
    public static int uprisingMonth; //The Month that the Game Ends On
    public static int uprisingDay; //The Day of the Uprising Month that the Game Ends On

    //Reputation and Faction Stuff
    public static Dictionary<string, Faction> factionList = new Dictionary<string, Faction>();
    public static int reputationCount;
    public static int playerReputationLevel;
    public static List<ReputationLevel> reputationLevels = new List<ReputationLevel>();

    //Party Stuff
    public static Party tonightsParty;
    public static int lastDay;
    public static int lastMonth;
    //For the RSVP Sysstem
    public static int partyRowID;
    public static int partyColumnID;
    //For the Rooms
    public static List<string> roomAdjectiveList = new List<string>();
    public static List<string> roomNounList = new List<string>();
    //For Dispositions
    public static List<Disposition> dispositionList = new List<Disposition>();
    //For Guests
    public static List<string> femaleTitleList = new List<string>();
    public static List<string> femaleFirstNameList = new List<string>();
    public static List<string> maleTitleList = new List<string>();
    public static List<string> maleFirstNameList = new List<string>();
    public static List<string> lastNameList = new List<string>();

    //Conversation Intro Texts
    public static List<string> conversationIntroList = new List<string>();
    public static List<string> hostRemarkIntroList = new List<string>();

    //Outfit Stuff. The Values start at -1 because I can't use null and I need an ID number that will never appear in the list.
    public static int partyOutfitID;
    public static int partyAccessoryID;
    //Used for seeing if the same Outfit was used twice in a row
    public static int lastPartyOutfitID;
    public static bool woreSameOutfitTwice;
    public static int noveltyDamage;

    //Style Stuff
    public static string currentStyle;
    public static string nextStyle;
    public static int lastStyleSwitch;
    public static int nextStyleSwitch;
    public static double outOfStylePriceMultiplier; //Effect on Outfit price it gets for being out of Style

    //Event Stuff. Can be "party" or "night";
    public static int eventChance; // 25 = 25% Chance of a Night Time Event, etc...
    public static Event selectedEvent;
    public static int totalPartyEventWeight;
    public static int totalNightEventWeight;
    public static int totalIntroEventWeight;

    //Servant Stuff
    public static Dictionary<string, Servant> servantDictionary = new Dictionary<string, Servant>();
    public static double seamstressDiscount; //Seamstress Price will be 80% of normal price.

    //UI Stuff
    public static int activeModals;
    public static int displayMonthInt;

    //Calendar Stuff
    public static Calendar calendar;
    public static int gameLengthMonths;
    public static int gameLengthDays;

    //Gossip Inventory
    public static List<Gossip> gossipInventory = new List<Gossip>();

    //Enemy Inventory is handled by the Enemy Inventory Class

    //Pierre Quest Inventory and related Stuff
    public static int nextQuestDay;
    public static int lastQuestDay;
    public static List<PierreQuest> pierreQuestInventory = new List<PierreQuest>();

    //End Game Victory and Loss Stuff
    public static string victoriousPower;
    public static string victoryDegree;
    public static string playerVictoryStatus;
    public static string playerAllegiance;

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print("Duplicate Game Data container self-destructing!");
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        // If we go to the main menu then Reset all the Game Data Values
        if (SceneManager.GetActiveScene().name == "Start Menu")
        {
            ResetValues();
        }
    }

    public void ResetValues()
    {
        moneyCount = 600;
        currentMonth = 0; //Month of the current Year (Also Zero Indexed)
        currentDay = 0; //Day of the current Month (Zero Indexed)
        startMonthInt = currentMonth;
        uprisingMonth = 0;
        uprisingDay = Random.Range(25, 31);

        //Faction Stuff
        factionList.Add("Crown", new Faction("Crown", 100, 100, 100));
        factionList.Add("Church", new Faction("Church", 100, -100, 60));
        factionList.Add("Military", new Faction("Military", 0, 0, 0));
        factionList.Add("Bourgeoisie", new Faction("Bourgeoisie", -100, 100, -60));
        factionList.Add("Third Estate", new Faction("Third Estate", -100, -100, -100));

        //Reputation
        reputationCount = 21;
        reputationLevels.Clear();
        reputationLevels.Add(new ReputationLevel(0, "Disregarded", 0, -10, 0));
        reputationLevels.Add(new ReputationLevel(1, "Known", 20, 0, 0));
        reputationLevels.Add(new ReputationLevel(2, "Accepted", 50, 5, 0));
        reputationLevels.Add(new ReputationLevel(3, "Liked", 100, 10, 0));
        reputationLevels.Add(new ReputationLevel(4, "Praised", 150, 15, 0));
        reputationLevels.Add(new ReputationLevel(5, "Favored", 200, 20, 0));
        reputationLevels.Add(new ReputationLevel(6, "Admired", 250, 25, 1));
        reputationLevels.Add(new ReputationLevel(7, "Esteemed", 300, 30, 1));
        reputationLevels.Add(new ReputationLevel(8, "Celebrated", 350, 35, 1));
        reputationLevels.Add(new ReputationLevel(9, "Beloved", 400, 40, 2));
        reputationLevels.Add(new ReputationLevel(10, "Impossible", 99999, 40, 2)); //Just there to put an upper limit on the whole thing

        //Party Stuff
        tonightsParty = null;
        lastDay = 0;
        lastMonth = currentMonth;
        //For the Rooms
        roomAdjectiveList.Clear();
        roomAdjectiveList.Add("Lovely");
        roomAdjectiveList.Add("Shady");
        roomAdjectiveList.Add("Gaudy");
        roomAdjectiveList.Add("Tasteful");
        roomAdjectiveList.Add("Smokey");
        roomAdjectiveList.Add("Dim");
        roomAdjectiveList.Add("Tasteless");
        roomAdjectiveList.Add("Crowded");
        roomAdjectiveList.Add("Sparse");
        roomAdjectiveList.Add("Quiet");
        roomAdjectiveList.Add("Loud");
        roomAdjectiveList.Add("Drunken");
        roomAdjectiveList.Add("Riotous");
        roomAdjectiveList.Add("Peaceful");
        roomAdjectiveList.Add("Sedate");

        roomNounList.Clear();
        roomNounList.Add("Dining Room");
        roomNounList.Add("Ball Room");
        roomNounList.Add("Smoking Room");
        roomNounList.Add("Study");
        roomNounList.Add("Terrace");
        roomNounList.Add("Garden");
        roomNounList.Add("Living Room");
        roomNounList.Add("Gallery");
        roomNounList.Add("Banquet Hall");
        roomNounList.Add("Parlor");
        roomNounList.Add("Billiards Room");

        //For Dispositions
        dispositionList.Clear();
        dispositionList.Add(new Disposition("Politics", Color.red, "Politics", "Theater"));
        dispositionList.Add(new Disposition("Theater", Color.magenta, "Theater", "Gossip"));
        dispositionList.Add(new Disposition("Gossip", Color.blue, "Gossip", "Music"));
        dispositionList.Add(new Disposition("Music", Color.yellow, "Music", "Politics"));

        //For Guests
        femaleTitleList.Clear();
        femaleTitleList.Add("Duchess");
        femaleTitleList.Add("Baroness");
        femaleTitleList.Add("Vis-Countess");
        femaleTitleList.Add("Countess");
        femaleTitleList.Add("Professor");

        maleTitleList.Clear();
        maleTitleList.Add("Duke");
        maleTitleList.Add("Baron");
        maleTitleList.Add("Vis-Count");
        maleTitleList.Add("Count");
        maleTitleList.Add("Professor");

        femaleFirstNameList.Clear();
        femaleFirstNameList.Add("Agnes");
        femaleFirstNameList.Add("Juliet");
        femaleFirstNameList.Add("Audrey");
        femaleFirstNameList.Add("Isabella");
        femaleFirstNameList.Add("Maria");
        femaleFirstNameList.Add("Mary");
        femaleFirstNameList.Add("Margaret");
        femaleFirstNameList.Add("Catherine");
        femaleFirstNameList.Add("Jeanne");
        femaleFirstNameList.Add("Eleanor");
        femaleFirstNameList.Add("Joanna");
        femaleFirstNameList.Add("Anne");
        femaleFirstNameList.Add("Suzanne");
        femaleFirstNameList.Add("Marie Therese");
        femaleFirstNameList.Add("Carmen");

        maleFirstNameList.Clear();
        maleFirstNameList.Add("Jean");
        maleFirstNameList.Add("Daniel");
        maleFirstNameList.Add("Henri");
        maleFirstNameList.Add("Pierre");
        maleFirstNameList.Add("Bernard");
        maleFirstNameList.Add("Richard");
        maleFirstNameList.Add("Mathieu");
        maleFirstNameList.Add("Jean-Pierre");
        maleFirstNameList.Add("Gaspard");
        maleFirstNameList.Add("Ferdinand");
        maleFirstNameList.Add("Charles");
        maleFirstNameList.Add("Claude");
        maleFirstNameList.Add("Oscar");
        maleFirstNameList.Add("Emile");

        lastNameList.Clear();
        lastNameList.Add("Albert");
        lastNameList.Add("Albon");
        lastNameList.Add("Amboise");
        lastNameList.Add("Bernadotte");
        lastNameList.Add("Conradin");
        lastNameList.Add("Dampierre");
        lastNameList.Add("Evreux");
        lastNameList.Add("Foix");
        lastNameList.Add("Gontaut");
        lastNameList.Add("Hottinguer");
        lastNameList.Add("Ingelger");
        lastNameList.Add("Lusignan");
        lastNameList.Add("Maiziere");
        lastNameList.Add("Montfort");
        lastNameList.Add("Noailles");
        lastNameList.Add("Orleans");
        lastNameList.Add("Poitiers");
        lastNameList.Add("Rouergue");
        lastNameList.Add("Taillefer");
        lastNameList.Add("Uzes");
        lastNameList.Add("Valois");

        //Conversation Intro Texts
        conversationIntroList.Clear();
        conversationIntroList.Add("Ah, have we met?");
        conversationIntroList.Add("And who might you be?");
        conversationIntroList.Add("Charmed, I'm sure...");
        conversationIntroList.Add("Oh, I've heard so much about you!");
        conversationIntroList.Add("You simply must meet my friends!");

        hostRemarkIntroList.Clear();
        hostRemarkIntroList.Add("Have you enjoyed yourself so far?");
        hostRemarkIntroList.Add("What do you think of the new king?");
        hostRemarkIntroList.Add("Has the music been to your liking?");
        hostRemarkIntroList.Add("Have you seen latest opera yet?");
        hostRemarkIntroList.Add("Wherever did you get that dress?");

        //Outfit Stuff. The Values start at -1 because I can't use null and I need an ID number that will never appear in the list.
        partyOutfitID = -1;
        partyAccessoryID = -1;
        //Used for seeing if the same Outfit was used twice in a row
        lastPartyOutfitID = -1;
        woreSameOutfitTwice = false;
        noveltyDamage = -20;

        //Style Stuff
        currentStyle = "Frankish";
        nextStyle = "Catalan";
        lastStyleSwitch = 0;
        nextStyleSwitch = Random.Range(6, 9);
        outOfStylePriceMultiplier = 0.75; //Effect on Outfit price it gets for being out of Style

        //Event Stuff. Can be "party" or "night";
        eventChance = 20; // 25 = 25% Chance of an event, etc...
        selectedEvent = null;

        //Servant Stuff
        servantDictionary.Clear();
        servantDictionary.Add("Camille", new Servant("Camille"));
        servantDictionary.Add("Seamstress", new Servant("Amelia"));
        servantDictionary.Add("Tailor", new Servant("Maurice"));
        servantDictionary.Add("Spymaster", new Servant("Thérèse"));
        servantDictionary.Add("Bodyguard", new Servant("Hansel"));
        seamstressDiscount = 0.8; //Seamstress Price will be 80% of normal price.

        //UI Stuff
        activeModals = 0;
        displayMonthInt = currentMonth;

        //Calendar Stuff
        calendar = new Calendar();
        gameLengthMonths = 4;
        for (int i = 0; i < (startMonthInt + gameLengthMonths); i++)
        {
            gameLengthDays = gameLengthDays + calendar.monthList[GameData.startMonthInt + i].days;
        }

        //Gossip Inventory
        gossipInventory.Clear();

        //Enemy Stuff
        if(EnemyInventory.enemyInventory != null)
        {
            EnemyInventory.enemyInventory.Clear();
        }

        //Pierre Quest Inventory and Related Stuff
        lastQuestDay = 0;
        nextQuestDay = Random.Range(3, 6);

        //End Game Victory and Loss Stuff
        victoriousPower = "";
        victoryDegree = "";
        playerVictoryStatus = "";
        playerAllegiance = "";
    }
}
