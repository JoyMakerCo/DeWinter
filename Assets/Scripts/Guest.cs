using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Guest {

    //General Settings
    public string name;
    public int dispositionInt;
    public Disposition disposition;
    public bool dispositionRevealed;
    public Faction faction;
    public bool isFemale; //Determines the gender of the Guest
    public int imageInt;
    //Opinion Boredom and Interest Stuff
    public float currentOpinion;
    public float maxOpinion;
    public float maxInterestTimer;
    public float currentInterestTimer;
    public bool attackTimerWaiting; //Only used for Enemies
    public int attackNumber;
    public int lockedInState = 0; //0 for Active, 1 for Charmed and -1 for Put Off

    //Enemy Stuff
    public bool isEnemy;
    
    //Generates a random regular Guest
    public Guest()
    {
        dispositionInt = Random.Range(0, 4);
        disposition = GameData.dispositionList[dispositionInt];
        currentOpinion = Random.Range(25,51);
        maxOpinion = 100;
        maxInterestTimer = 10;
        currentInterestTimer = 10;
        attackTimerWaiting = false; //Only used for Enemies
        dispositionRevealed = false;
        isEnemy = false;
        isFemale = GenderDeterminer();
        if (isFemale)
        {
            imageInt = Random.Range(0, 2);
        } else
        {
            imageInt = Random.Range(0, 2);
        }
        name = GenerateName(); // Have to Generate the Name after the Gender
    }

    //Generates a Guest with a specific Opinion level and Interest Timer, used by the Room class to set difficulty
    public Guest(int opinion, int interest)
    {      
        dispositionInt = Random.Range(0, 4);
        disposition = GameData.dispositionList[dispositionInt];
        currentOpinion = opinion;
        maxOpinion = 100;
        maxInterestTimer = interest;
        currentInterestTimer = maxInterestTimer;
        attackTimerWaiting = false; //Only used for Enemies
        dispositionRevealed = false;
        isEnemy = false;
        isFemale = GenderDeterminer();
        if (isFemale)
        {
            imageInt = Random.Range(0, 4);
        }
        else
        {
            imageInt = Random.Range(0, 5);
        }
        name = GenerateName();  // Have to Generate the Name after the Gender
    }

    //Generates a Guest who is an Enemy, Enemy is the greater class tracking the Persistent Enemy, Guests with a true 'isEnemy' bool are here for the actual battles
    public Guest(Enemy enemy)
    {
        dispositionInt = enemy.dispositionInt;
        disposition = enemy.disposition;
        currentOpinion = Random.Range(25, 51);
        maxOpinion = 100;
        maxInterestTimer = 10;
        currentInterestTimer = 10;
        attackTimerWaiting = false; //Only used for Enemies
        dispositionRevealed = false;
        isEnemy = true;
        isFemale = enemy.isFemale;
        imageInt = enemy.imageInt;
        name = enemy.Name(); // Have to Generate the Name after the Gender
    }

    string GenerateName()
    {
        string title;
        string firstName;
        if (isFemale)
        {
            title = GameData.femaleTitleList[Random.Range(0, GameData.femaleTitleList.Count)];
            firstName = GameData.femaleFirstNameList[Random.Range(0, GameData.femaleFirstNameList.Count)];
        } else
        {
            title = GameData.maleTitleList[Random.Range(0, GameData.maleTitleList.Count)];
            firstName = GameData.maleFirstNameList[Random.Range(0, GameData.maleFirstNameList.Count)];
        }
        string lastName = GameData.lastNameList[Random.Range(0, GameData.lastNameList.Count)];
        return title + " " + firstName + " de " + lastName;
    }

    bool GenderDeterminer()
    {
        int genderInt = Random.Range(1, 3);
        if (genderInt == 1)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public int AttackReaction(Guest charmedGuest)
    {
        if (lockedInState != 1) //If the Enemy is not Dazed
        {
            //1 = Monopolize Conversation (Lose a Turn)
            //2 = Rumor Monger (Lower the Opinion of all uncharmed Guests)
            //3 = Belittle (Sap your Confidence)
            //4 = Antagonize (Uncharm a Charmed Guest, if there is one)
            if (charmedGuest != null)
            {
                return Random.Range(1, 5);
            } else
            {
                return Random.Range(1, 4);
            }
        }
        else
        {
            return 0; //No attack because the Enemy is Dazed
        }
    }
}
