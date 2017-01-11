using UnityEngine;
using System.Collections;

public class Faction {

    string name;
    public int playerReputation; //What's the Player's Rep with this faction? Raw Number
    private int playerReputationLevel; //The Player's 'Reputation Level'. Intended to make the Reputation system a little more understandable
    private int[] playerReputationLevelRequirements;
    float power; //How powerful is this faction in the game?
    public string knownPower = "Unknown"; //Used in the 'Test the Waters' screen
    public int powerKnowledgeTimer;
    public int modestyLike; //How modest do they like their outfits?
    public int luxuryLike; //How luxurious do they like their outfits?
    float allegiance; //-100 means the Faction is devoted to the Revolution, 100 means they are devoted to the Crown
    public string knownAllegiance = "Unknown"; //Used in the 'Test the Waters' screen
    public int allegianceKnowledgeTimer;

    public Faction (string nme, int mL, int lL, int all)
    {
        name = nme;
        modestyLike = mL;
        luxuryLike = lL;
        power = 10;
        allegiance = all;
        playerReputation = 0;
        playerReputationLevelRequirements = new int[10];
        StockPlayerReputationLevelRequirements();
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
        playerReputationLevelRequirements[0] = 0;
        playerReputationLevelRequirements[1] = 20;
        playerReputationLevelRequirements[2] = 50;
        playerReputationLevelRequirements[3] = 100;
        playerReputationLevelRequirements[4] = 150;
        playerReputationLevelRequirements[5] = 200;
        playerReputationLevelRequirements[6] = 250;
        playerReputationLevelRequirements[7] = 300;
        playerReputationLevelRequirements[8] = 350;
        playerReputationLevelRequirements[9] = 400;
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
                return 2;
            case 3:
                return 2;
            case 4:
                return 3;
            case 5:
                return 3;
            case 6:
                return 4;
            default:
                return 4;
        }

    }

    public string Name()
    {
        return name;
    }

    public float Allegiance()
    {
        return allegiance;
    }

    public void ChangeAllegiance(int changeAmount)
    {
        if (name != "Revolution" || name != "Crown")
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

    public void CalculateKnownPower()
    {
        //Text Conversion
        if (power >= 90)
        {
            knownPower = "Dominating";
        }
        else if (power < 90 && power >= 60)
        {
            knownPower = "Formidable";
        }
        else if (power < 60 && power > 40)
        {
            knownPower = "Significant";
        }
        else if (power <= 40 && power > 10)
        {
            knownPower = "Weak";
        }
        else if (10 <= power)
        {
            knownPower = "Insignificant";
        }
        powerKnowledgeTimer = 0;
    }

    public void CalculateKnownAllegiance()
    {
        //Text Conversion
        if (allegiance > 80)
        {
            knownAllegiance = "Ultra Monarchist";
        }
        else if (allegiance > 50 && allegiance <= 80)
        {
            knownAllegiance = "Monarchist";
        }
        else if (allegiance > 20 && allegiance <= 50)
        {
            knownAllegiance = "Leaning Towards the Monarchy";
        }
        else if (allegiance >= -20 && allegiance <= 20)
        {
            knownAllegiance = "Undecided";
        }
        else if (allegiance >= -50 && allegiance < -20)
        {
            knownAllegiance = "Leaning Towards the Revolution";
        }
        else if (allegiance >= -80 && allegiance < -50)
        {
            knownAllegiance = "Revolutionary";
        } 
        else if (allegiance < -80)
        {
            knownAllegiance = "Radical Revolutionary";
        }
        allegianceKnowledgeTimer = 0;
    }
}
