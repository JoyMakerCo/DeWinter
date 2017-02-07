using UnityEngine;
using System.Collections;

public class Enemy
{
    //General Settings
    private string name;
    private string flavorText;
    public int dispositionInt;
    public Disposition disposition;
    public string Faction;
    public bool isFemale; //Determines the gender of the Guest
    public int imageInt;

    //Generates a Random Enemy from a particular Faction
    public Enemy(string faction)
    {
        dispositionInt = Random.Range(0, 4);
        disposition = GameData.dispositionList[dispositionInt];
        Faction = faction;
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
	public Enemy(string faction, string nme, bool gen)
    {
        dispositionInt = Random.Range(0, 4);
        disposition = GameData.dispositionList[dispositionInt];
		Faction = faction;
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
