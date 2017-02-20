using UnityEngine;
using System.Collections;
using DeWinter;

public class Notable {

    //General Settings
    public string name;
    public FactionVO faction;
    public bool isFemale; //Determines the gender of the Guest
    public bool enemy;
    public int imageInt;
    //Opinion Boredom and Interest Stuff
    public float currentOpinion;
    public float maxOpinion;
    public float maxInterestTimer;
    public float currentInterestTimer;
    public bool interestTimerWaiting;
    public int lockedInState = 0; //0 for Active, 1 for Charmed and -1 for Put Out
    //Disposition Stuff
    public int dispositionInt;
    public float dispositionTimerSwitchMax;
    public Disposition disposition;
    public bool dispositionRevealed;
    //Host Remark Stuff
    public float nextHostRemarkTimer;
    public float hostRemarkCompletionTimerMax;
    public float hostRemarkCompletionTimerCurrent;

    //Generates a random regular Notable
    public Notable()
    {
        dispositionInt = Random.Range(0, 4);
        disposition = GameData.dispositionList[dispositionInt];
        currentOpinion = Random.Range(25, 51);
        maxOpinion = 225;
        maxInterestTimer = 4;
        currentInterestTimer = 4;
        interestTimerWaiting = false;
        enemy = false;
        dispositionRevealed = false;
        isFemale = GenderDeterminer();
        if (isFemale)
        {
            imageInt = Random.Range(0, 3);
        }
        else
        {
            imageInt = Random.Range(0, 4);
        }
        name = GenerateName(); // Have to Generate the Name after the Gender
        dispositionTimerSwitchMax = Random.Range(4, 7);
        nextHostRemarkTimer = 10;
        hostRemarkCompletionTimerMax = 6;
        hostRemarkCompletionTimerCurrent = hostRemarkCompletionTimerMax;
}

    //Generates a Notable with a specific Opinion level and Interest Timer, used by the Room class to set difficulty
    public Notable(int opinion, int interest)
    {
        dispositionInt = Random.Range(0, 4);
        disposition = GameData.dispositionList[dispositionInt];
        currentOpinion = opinion;
        maxOpinion = 250;
        maxInterestTimer = interest;
        currentInterestTimer = maxInterestTimer;
        enemy = false;
        dispositionRevealed = false;
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
        nextHostRemarkTimer = 10;
        hostRemarkCompletionTimerMax = 6;
        hostRemarkCompletionTimerCurrent = hostRemarkCompletionTimerMax;
    }

    string GenerateName()
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

    bool GenderDeterminer()
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

    public IEnumerator TimerWait()
    {
        Debug.Log("Timer Started!");
        yield return new WaitForSeconds(0.75f);
        interestTimerWaiting = false;
    }

    public void ChangeDisposition()
    {
        int newDispositionInt = Random.Range(0, 4);
        dispositionInt = newDispositionInt;
        disposition = GameData.dispositionList[dispositionInt];
        dispositionTimerSwitchMax = Random.Range(4.0f, 7.0f);
    }
}

