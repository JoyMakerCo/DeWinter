using UnityEngine;
using System.Collections;

public class Servant {

    string slot; //What role do they fill? Seamstress? Spymaster? Bodyguard?
    string name; //What is their given name?
    string title; //Title, just used in name stuff
    string description; //What this servant can do for the Player
    int wage; //How much they cost per week
    bool introduced; //Has this servant been introduced yet?
    bool hired; //Are you employing this Servant right now?

	public Servant(string n)
    {
        name = n;
        introduced = false;
        hired = false;
        GenerateServant();
    }

    void GenerateServant()
    {
        switch (name)
        {
            case "Camille":
                //TODO: Can't be fired include the 'Attempted Firing' Event
                slot = "Handmaiden";
                title = "Handmaiden";
                description = "- Your loyal Handmaiden" +
                              "\n- Handles basic day-to-day tasks";
                introduced = true;
                hired = true;
                wage = 5;
                break;
            case "Amelia":
                slot = "Clothier";
                title = "Seamstress";
                description = "- Gives a discount in the Wardrobe Store" +
                              "\n- Can tell you the next upcoming Fashion Trend" +
                              "\n- Can Sew new Outfits";
                wage = 50;
                break;
            case "Maurice":
                slot = "Clothier";
                title = "Tailor";
                description = "- Gives a discount in the Wardrobe Store" +
                              "\n- Can tell you the next upcoming Fashion Trend" +
                              "\n- Can Spruce Up tired old Outfits";
                wage = 50;
                break;
            case "Thérèse":
                slot = "Subterfuge";
                title = "Spymaster";
                description = "- Gives Free Political Updates" +
                              "\n- Lets you see if your Enemies are attending a Party" +
                              "\n- Protects Against Skullduggery";
                wage = 50;
                break;
            case "Hansel":
                slot = "Escort";
                title = "Mercenary";
                description = "- A burly Swiss mercenary" + 
                              "\n- Protects you from assassins and boors" +
                              "\n- Can intimdate people during Events";
                wage = 60;
                break;
        }
    }

    public void Introduce()
    {
        introduced = true;
    }

    public bool Introduced()
    {
        return introduced;
    }

    public void Hire()
    {
        if (introduced)
        {
            bool slotTaken = false;
            foreach (string k in GameData.servantDictionary.Keys)
            {
                Servant s = GameData.servantDictionary[k];
                if(slot == s.slot && name != s.name && s.hired) //If they share the same slot, but not the same name and this Servant is already Hired
                {
                    slotTaken = true;
                }
            }
            if (!slotTaken)
            {
                hired = true;
            } else
            {
                Debug.Log("Can't Hire that Servant, there's someone in that slot already");
            }
        } else
        {
            Debug.Log("Can't Hire a Servant who hasn't been introduced yet");
        }
        Debug.Log(NameAndTitle() + " Hired!");
    }

    public bool Hired()
    {
        return hired;
    }

    public void Fire()
    {
        introduced = false;
        hired = false;
        Debug.Log(NameAndTitle() + " Fired!");
    }

    public int Wage()
    {
        return wage;
    }

    public string Name()
    {
        return name;
    }

    public string NameAndTitle()
    {
        return name + ", the " + title;
    }

    public string Slot()
    {
        return slot;
    }

    public string Description()
    {
        string desc = description;
        desc += "\n- Costs " + wage.ToString("£" + "#,##0") + "/Week";
        return desc;
    }
}
