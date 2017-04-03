using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationLevel {

    private int level;
    private string name;
    private int requiredReputation;
    private int confidenceBonus;
    private int mandatoryPartyInviteImportance;

    public ReputationLevel(int l, string n, int rR, int cB, int mI)
    {
        level = l;
        name = n;
        requiredReputation = rR;
        confidenceBonus = cB;
        mandatoryPartyInviteImportance = mI;
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    //How much Reputation is reqruired for this Reputation Level?
    public int RequiredReputation()
    {
        return requiredReputation;
    }

    //What Confidence Bonus does this Level grant during Parties?
    public int ConfidenceBonus()
    {
        return confidenceBonus;
    }

    //What Importance of Parties do Players get invited to, regardless of Faction Level?
    public int PartyInviteImportance()
    {
        return mandatoryPartyInviteImportance;
    }

    public string BenefitString()
    {
        switch (level)
        {
            case 0:
                return "Level 0: No Benefits";
            case 1:
                return "Level 1: Pierre will approach you with special Gossip Requests";
            case 2:
                return "Level 2: You can get Reparte Bonuses in Conversations";
            case 3:
                return "Level 3: +5% Chance to Seduce a Party Host";
            case 4:
                return "Level 4: Your Remarks now a 25% stronger effect on your Enemies";
            case 5:
                return "Level 5: You now have 10% more time in Turns against a Host and to reply to their Remarks";
            case 6:
                return "Level 6: You're invited to all Decent Parties, regardless of Faction";
            case 7:
                return "Level 7: Another +5% Chance to Seduce Party Hosts (+10% Total)";
            case 8:
                return "Level 8: Attending a Party with a coordinated Outfit and Accessory may cause Fashion to shift your way";
            default:
                return "Level 9: You're invited to all Decent Parties, regardless of Faction";
        }
    }
}
