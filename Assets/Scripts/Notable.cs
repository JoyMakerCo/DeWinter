using UnityEngine;
using System.Collections;
using DeWinter;

public class Notable : PartyGoer {

    public bool interestTimerWaiting;

    public LockedInState notableLockedInState;

    //Disposition Stuff
    public float dispositionTimerSwitchMax;
    
    //Host Remark Stuff
    public float nextHostRemarkTimer;
    public float hostRemarkCompletionTimerMax;
    public float hostRemarkCompletionTimerCurrent;

    //Generates a random regular Notable
    public Notable(string f)
    {
        faction = f;
        dispositionInt = Random.Range(0, 4);
        disposition = GameData.dispositionList[dispositionInt];
        currentOpinion = Random.Range(25, 51);
        maxOpinion = 225;
        notableLockedInState = LockedInState.Interested;
        maxInterestTimer = 4;
        currentInterestTimer = 4;
        interestTimerWaiting = false;
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
        Name = GenerateName(); // Have to Generate the Name after the Gender
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
        notableLockedInState = LockedInState.Interested;
        maxInterestTimer = interest;
        currentInterestTimer = maxInterestTimer;
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
        Name = GenerateName();  // Have to Generate the Name after the Gender
        nextHostRemarkTimer = 10;
        hostRemarkCompletionTimerMax = 6;
        hostRemarkCompletionTimerCurrent = hostRemarkCompletionTimerMax;
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