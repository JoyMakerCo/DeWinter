using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ambition;

namespace Ambition
{
	public class Guest : PartyGoer
	{
	    public bool attackTimerWaiting; //Only used for Enemies
	    public int attackNumber;

	    public LockedInState lockedInState;

	    //Enemy Stuff
	    public bool isEnemy;
	    
	    //Generates a random regular Guest
	    public Guest() : base()
	    {
	        dispositionInt = Random.Range(0, 4);
	        disposition = GameData.dispositionList[dispositionInt];
	        lockedInState = LockedInState.Interested;
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
	        Name = GenerateName(); // Have to Generate the Name after the Gender
	    }

	    //Generates a Guest with a specific Opinion level and Interest Timer, used by the Room class to set difficulty
	    public Guest(int opinion, int interest) : base()
	    {      
	        dispositionInt = Random.Range(0, 4);
	        disposition = GameData.dispositionList[dispositionInt];
	        lockedInState = LockedInState.Interested;
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
	        Name = GenerateName();  // Have to Generate the Name after the Gender
	    }

	    //Generates a Guest who is an Enemy, Enemy is the greater class tracking the Persistent Enemy, Guests with a true 'isEnemy' bool are here for the actual battles
	    public Guest(Enemy enemy) : base()
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
	        isFemale = enemy.IsFemale;
	        imageInt = enemy.imageInt;
	        Name = enemy.Name; // Have to Generate the Name after the Gender
	    }

	    public int AttackReaction(Guest charmedGuest)
	    {
	        if (lockedInState != LockedInState.Charmed) //If the Enemy is not Dazed
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
}