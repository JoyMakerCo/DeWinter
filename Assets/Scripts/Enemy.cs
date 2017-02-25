using UnityEngine;
using System.Collections;

public class Enemy : PartyGoer {

    //General Settings
    private string flavorText;

    //Generates a Random Enemy from a particular Faction
    public Enemy(Faction fac)
    {
        dispositionInt = Random.Range(0, 4);
        disposition = GameData.dispositionList[dispositionInt];
        faction = fac;
        isFemale = GenderDeterminer();
        if (isFemale)
        {
            imageInt = Random.Range(0, 4);
        }
        else
        {
            imageInt = Random.Range(0, 5);
        }
        name = GenerateName(); // Have to Generate the Name after the Gender
        flavorText = GenerateFlavorText();
    }

    //Generates an Enemy with a Particular Name, Faction and Gender
    public Enemy(Faction fac, string nme, bool gen)
    {
        dispositionInt = Random.Range(0, 4);
        disposition = GameData.dispositionList[dispositionInt];
        faction = fac;
        isFemale = gen;
        if (isFemale)
        {
            imageInt = Random.Range(0, 4);
        }
        else
        {
            imageInt = Random.Range(0, 5);
        }
        name = nme;
        flavorText = GenerateFlavorText();
    }

    public string Name()
    {
        return name;
    }

    public string FlavorText()
    {
        return flavorText;
    }

    string GenerateFlavorText()
    {
        return "This person is a great big jerk";
    }
}
