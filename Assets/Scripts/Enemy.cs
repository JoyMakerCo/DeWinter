using System;
using System.Collections;
using Ambition;

public class Enemy
{
    //General Settings
    public string Name;
    private string flavorText;
    public int dispositionInt;
    public Disposition disposition;
    public string Faction;
    public bool IsFemale; //Determines the gender of the Guest
    public int imageInt;

	//Generates an Enemy with a Particular Name, Faction and Gender
    public Enemy(string faction, string name, bool isFemale)
    {
    	Random rnd = new Random();
		PartyModel model = AmbitionApp.GetModel<PartyModel>();

		disposition = model.Dispositions[rnd.Next(model.Dispositions.Length)];
        Faction = faction;
        IsFemale = isFemale;
		imageInt = rnd.Next(IsFemale ? 4 : 5);
        Name = name;
        flavorText = GenerateFlavorText();
    }

    //Generates a Random Enemy from a particular Faction
	public Enemy(string faction=null)
	{
		Random rnd = new Random();
		PartyModel model = AmbitionApp.GetModel<PartyModel>();

		disposition = model.Dispositions[rnd.Next(model.Dispositions.Length)];
        Faction = faction;
        IsFemale = GenderDeterminer();
		imageInt = rnd.Next(IsFemale ? 4 : 5);
        Name = GenerateName();
        flavorText = GenerateFlavorText();
	}

    private string GenerateName()
    {
    	PartyModel model = AmbitionApp.GetModel<PartyModel>();
        string title;
        string firstName;
        Random rnd = new Random();
        if (IsFemale)
        {
            title = model.FemaleTitles[rnd.Next(model.FemaleTitles.Length)];
			firstName = model.FemaleNames[rnd.Next(model.FemaleNames.Length)];
        }
        else
        {
			title = model.MaleTitles[rnd.Next(model.MaleTitles.Length)];
			firstName = model.MaleNames[rnd.Next(model.MaleNames.Length)];
        }
		string lastName = model.LastNames[rnd.Next(model.LastNames.Length)];
        return title + " " + firstName + " de " + lastName;
    }

	private static bool GenderDeterminer()
    {
		return (new Random()).Next(2) == 0;
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