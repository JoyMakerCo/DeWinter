using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour {

    public Room myRoom;
    public RoomManager roomManager;
    Text myDescriptionText;
    Outline myOutline; // Indicates that the Player is there
    Image myImage;
    Image myPunchBowlImage;

	// Use this for initialization
	void Start () {
        myDescriptionText = this.transform.Find("DescriptionText").GetComponent<Text>();
        myOutline = this.GetComponent<Outline>();
        myPunchBowlImage = this.transform.Find("PunchBowlImage").GetComponent<Image>();
	    myImage = this.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (myRoom.revealed) //If the Room has been Revealed
	    {
	        myImage.color = Color.white;
	        myDescriptionText.color = Color.white;
            if (!myRoom.entrance)
	        {
	            if (!myRoom.hostHere)
	            {
	                if (!myRoom.cleared)
	                {
                        if (myRoom.starRating != 0) //If it's not an Event Room (1-5 Stars)
                        {
                            myDescriptionText.text = myRoom.name + "\n(" + myRoom.starRating.ToString() + " Stars)";
                        } else // If it is an Event Room (0 Stars)
                        {
                            myDescriptionText.text = myRoom.name + "\n(Event)";
                        }
	                }
	                else
	                {
	                    myDescriptionText.text = myRoom.name + "\n(Cleared)";
	                }

	            }
	            else
	            {
	                if (!myRoom.cleared)
	                {
	                    myDescriptionText.text = myRoom.name + "\n(Host Here)";
	                }
	                else
	                {
	                    myDescriptionText.text = myRoom.name + "\n(Host Cleared)";
	                }
	            }
	        }
	        else
	        {
	            myDescriptionText.text = myRoom.name;
	        }
	        //Is the player here? If so, Outline
	        if (myRoom.playerHere)
	        {
	            myOutline.effectColor = Color.black;
	        }
	        else if (myRoom.playerAdjacent)
	        {
	            myOutline.effectColor = Color.yellow;
	        }
	        else if (myRoom.hostHere)
	        {
	            myOutline.effectColor = Color.red;
	        }
	        else
	        {
	            myOutline.effectColor = Color.clear;
	        }
	        //Is there a Punch Bowl? If so, display the Icon
	        if (myRoom.punchBowl)
	        {
	            myPunchBowlImage.color = Color.white;
	        }
	        else
	        {
	            myPunchBowlImage.color = Color.clear;
	        }
	    }
	    else //If the Room has yet to be revealed
	    {
	        myImage.color = Color.clear;
	        myPunchBowlImage.color = Color.clear;
	        myOutline.effectColor = Color.clear;
	        myDescriptionText.color = Color.clear;
	    }
    }

    public void Move()
    {
        if (myRoom.playerAdjacent && myRoom.party.turnsLeft > 0)
        {
            if (myRoom.starRating != 0)
            {
                roomManager.PlayerMovement(myRoom.xPos, myRoom.yPos);
                //Tell Room Manager to set the other Player here to False
                roomManager.ChoiceModal(myRoom.xPos, myRoom.yPos);
            } else
            {
                roomManager.PlayerMovement(myRoom.xPos, myRoom.yPos);
                //Tell Room Manager to set the other Player here to False
                roomManager.PartyEventModal(myRoom.xPos, myRoom.yPos);
            }
        } else
        {
            Debug.Log("Can't move there, sorry :(");
        }

    }
}
