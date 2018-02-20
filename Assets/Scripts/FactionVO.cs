using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class FactionVO
{
	[JsonProperty("Name")]
    public string Name;

	[JsonProperty("Modesty")]
	public int modestyLike; //How modest do they like their outfits?

	[JsonProperty("Luxury")]
	public int luxuryLike; //How luxurious do they like their outfits?

	[JsonProperty("Allegiance")]
	float allegiance; //-100 means the Faction is devoted to the Revolution, 100 means they are devoted to the Crown

	[JsonProperty("Power")]
	float power; //How powerful is this faction in the game?

	private int[] playerReputationLevelRequirements;

    public int playerReputation=0; //What's the Player's Rep with this faction? Raw Number
    private int playerReputationLevel; //The Player's 'Reputation Level'. Intended to make the Reputation system a little more understandable
    public string knownPower = "Unknown"; //Used in the 'Test the Waters' screen
    public int powerKnowledgeTimer;
    public string knownAllegiance = "Unknown"; //Used in the 'Test the Waters' screen
    public int allegianceKnowledgeTimer;

    public List<string> benefitsList = new List<string>();

    public FactionVO ()
    {
        power = 10;
        StockPlayerReputationLevelRequirements();
        StockBenefitsList();
    }

    public string Likes()
    {
        string likes;
        string ll;
        string ml;
        //----------
        if (luxuryLike > 0)
        {
            ll = "Luxurious";
        } else if (luxuryLike < 0 )
        {
            ll = "Vintage";
        } else
        {
            ll = "Doesn't Care"; // This will never get used, but it's here just in case...
        }
        //---------
        if (modestyLike > 0)
        {
            ml = "Modest";
        } else if (modestyLike < 0)
        {
            ml = "Racy";
        } else
        {
            ml = "Doesn't Care"; // This will never get used, but it's here just in case...
        }
        //---------
        if (luxuryLike == 0 && modestyLike == 0) //Military Only (Temporary)
        {
            likes = "They don't care about your clothes";
        } else
        {
            likes = ll + " and " + ml + " Outfits";
        }
        return likes;
    }

    public string Dislikes()
    {
        string dislikes;
        string ld;
        string md;
        //----------
        if (luxuryLike > 0)
        {
            ld = "Vintage";
        }
        else if (luxuryLike < 0)
        {
            ld = "Luxurious";
        }
        else
        {
            ld = "Doesn't Care"; // This will never get used, but it's here just in case...
        }
        //---------
        if (modestyLike > 0)
        {
            md = "Racy";
        }
        else if (modestyLike < 0)
        {
            md = "Modest";
        }
        else
        {
            md = "Doesn't Care"; // This will never get used, but it's here just in case...
        }
        //---------
        if (luxuryLike == 0 && modestyLike == 0)
        {
            dislikes = "They don't care about your clothes";
        }
        else
        {
            dislikes = ld + " and " + md + " Outfits";
        }
        return dislikes;
    }

    private void StockPlayerReputationLevelRequirements()
    {
		playerReputationLevelRequirements = new int[]{
			0, 20, 50, 100, 150, 200, 250, 300, 350, 400, int.MaxValue
		};
    }

    void StockBenefitsList()
    {
        benefitsList.Add("Level 0: Invited to Small " + Name + " Parties");
        benefitsList.Add("Level 1: Wine upon entering " + Name + " Parties");
        switch (Name)
        {
            case "Crown":
                benefitsList.Add("Level 2: Training in Courtly Dances");
                break;
            case "Church":
                benefitsList.Add("Level 2: Confessions allow Scandals to disappear faster");
                break;
            case "Military":
                benefitsList.Add("Level 2: A free bodyguard is provided");
                break;
            case "Bourgeoisie":
                benefitsList.Add("Level 2: The merchant stocks 4 Outfits per day instead of 3");
                break;
            case "Revolution":
                benefitsList.Add("Level 2: Selling Gossip to the press is now less risky");
                break;
        }
        benefitsList.Add("Level 3: Invited to Medium " + Name + " Parties, extra time at Small " + Name + " Parties");
        benefitsList.Add("Level 4: Extra attention from the drink servers at " + Name + " Parties");
        benefitsList.Add("Level 5: Always know the Power of the " + Name);
        benefitsList.Add("Level 3: Invited to Large " + Name + " Parties, extra time at Medium " + Name + " Parties");
        benefitsList.Add("Level 7: Always know Allegiance of the " + Name);
        benefitsList.Add("Level 8: The " + Name + " will come to your aid in an hour of great need.");
        switch (Name)
        {
            case "Crown":
                benefitsList.Add("Level 9: A title and Royal Allowance");
                break;
            case "Church":
                benefitsList.Add("Level 9: Confessions to a Cardinal lend you immunity to Scandal");
                break;
            case "Military":
                benefitsList.Add("Level 9: The Military has a ship waiting for you");
                break;
            case "Bourgeoisie":
                benefitsList.Add("Level 9: You are given influence at the Fashion Houses and may pick styles");
                break;
            case "Revolution":
                benefitsList.Add("Level 9: You are a member of the Revolution Council");
                break;
        }
    }

    public int PlayerReputationLevel()
    {
        int i = 0;
        while (playerReputation > playerReputationLevelRequirements[i])
        {
            //Debug.Log(name + " Faction Level: " + i);
            i++;
        }
        playerReputationLevel = i;
        return playerReputationLevel;
    }

    public int largestAllowableParty()
    {
        switch (PlayerReputationLevel())
        {
            case 0:
                return 1;
            case 1:
                return 1;
            case 2:
                return 1;
            case 3:
                return 2;
            case 4:
                return 2;
            case 5:
                return 2;
            case 6:
                return 3;
            default:
                return 3;
        }

    }

    public float Allegiance()
    {
        return allegiance;
    }

    public void ChangeAllegiance(int changeAmount)
    {
        if (Name != "Revolution" || Name != "Crown")
        {
            allegiance = Mathf.Clamp(allegiance + changeAmount, -100, 100);
        }
    }

    public void ChangePower(int changeAmount)
    {
        power = Mathf.Clamp(power + changeAmount, 0, 100);
    }

    public float Power()
    {
        return power;
    }

    public string PowerString()
    {
        //Text Conversion
        if (power >= 90)
        {
            return "Dominating";
        }
        else if (power < 90 && power >= 60)
        {
            return "Formidable";
        }
        else if (power < 60 && power > 40)
        {
            return "Significant";
        }
        else if (power <= 40 && power > 10)
        {
            return "Weak";
        }
        else
        {
            return "Insignificant";
        }
    }

    public void CalculateKnownPower()
    {
        knownPower = PowerString();
        powerKnowledgeTimer = 0;
    }

    public string AllegianceString()
    {
        //Text Conversion
        if (allegiance > 80)
        {
            return "Ultra Monarchist";
        }
        else if (allegiance > 50 && allegiance <= 80)
        {
            return "Monarchist";
        }
        else if (allegiance > 20 && allegiance <= 50)
        {
            return "Leaning Towards the Monarchy";
        }
        else if (allegiance >= -20 && allegiance <= 20)
        {
            return "Undecided";
        }
        else if (allegiance >= -50 && allegiance < -20)
        {
            return "Leaning Towards the Revolution";
        }
        else if (allegiance >= -80 && allegiance < -50)
        {
            return "Revolutionary";
        }
        else
        {
            return "Radical Revolutionary";
        }
    }

    public void CalculateKnownAllegiance()
    {
        knownAllegiance = AllegianceString();
        allegianceKnowledgeTimer = 0;
    }
}
