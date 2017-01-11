using UnityEngine;
using System.Collections;

public class Servant {

    string slot; //What role do they fill? Seamstress? Spymaster? Bodyguard?
    string name; //What is their given name?
    int wage; //How much they cost per week
    bool introduced; //Has this servant been introduced yet?
    bool hired; //Are you employing this Servant right now?

	public Servant(string s)
    {
        slot = s;
        introduced = false;
        hired = false;
        GenerateServant();
    }

    void GenerateServant()
    {
        switch (slot)
        {
            case "Handmaiden":
                name = "Camille";
                introduced = true;
                hired = true;
                wage = 5;
                break;
            case "Seamstress":
                name = "Amelia";
                wage = 50;
                break;
            case "Spymaster":
                name = "Thérèse";
                wage = 50;
                break;
            case "Bodyguard":
                name = "Hansel";
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
            hired = true;
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
        return name + ", the " + slot;
    }

    public string Slot()
    {
        return slot;
    }
}
