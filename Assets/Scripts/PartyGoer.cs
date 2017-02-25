using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyGoer {

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
    public enum lockedInState { Charmed, Interested, PutOff }

    protected string GenerateName()
    {
        string title;
        string firstName;
        if (isFemale)
        {
            title = GameData.femaleTitleList[Random.Range(0, GameData.femaleTitleList.Count)];
            firstName = GameData.femaleFirstNameList[Random.Range(0, GameData.femaleFirstNameList.Count)];
        }
        else
        {
            title = GameData.maleTitleList[Random.Range(0, GameData.maleTitleList.Count)];
            firstName = GameData.maleFirstNameList[Random.Range(0, GameData.maleFirstNameList.Count)];
        }
        string lastName = GameData.lastNameList[Random.Range(0, GameData.lastNameList.Count)];
        return title + " " + firstName + " de " + lastName;
    }

    protected bool GenderDeterminer()
    {
        int genderInt = Random.Range(1, 3);
        if (genderInt == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
