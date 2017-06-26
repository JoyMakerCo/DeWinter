using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public enum LockedInState { Charmed, Interested, PutOff };

	public class PartyGoer
	{
	    //General Settings
	    public string Name;
	    public int dispositionInt;
	    public Disposition disposition;
	    public bool dispositionRevealed;
	    public string faction;
	    public bool isFemale; //Determines the gender of the Guest
	    public int imageInt;
	    //Opinion Boredom and Interest Stuff
	    public float currentOpinion;
	    public float maxOpinion;
	    public float maxInterestTimer;
	    public float currentInterestTimer;

	    protected string GenerateName()
	    {
	        string title;
	        string firstName;
	        if (isFemale)
	        {
				title = GameData.femaleTitleList[Random.Range(0, GameData.femaleTitleList.Length)];
				firstName = GameData.femaleFirstNameList[Random.Range(0, GameData.femaleFirstNameList.Length)];
	        }
	        else
	        {
	            title = GameData.maleTitleList[Random.Range(0, GameData.maleTitleList.Length)];
				firstName = GameData.maleFirstNameList[Random.Range(0, GameData.maleFirstNameList.Length)];
	        }
			string lastName = GameData.lastNameList[Random.Range(0, GameData.lastNameList.Length)];
	        return title + " " + firstName + " de " + lastName;
	    }

	    protected bool GenderDeterminer()
	    {
	        return Random.Range(0, 2) == 0;
	    }
	}
}