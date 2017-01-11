using UnityEngine;
using System.Collections;

public class Accessory {

    string style; // Styles: Frankish, Venezian and Catalan
    int styleNumber;
    string type; //The kind of bonus the Accessory Grants. This also determines the name
    int typeNumber;
    int price;
    public int imageID;

    // Generate a random Accessory
    public Accessory()
    {
        RandomStyle();    
        RandomType();
        price = CalculatePrice();
        imageID = 0; //No Images Yet
    }

    // Generate a random Accessory of a particular Style
    public Accessory(string sty)
    {
        style = sty;
        RandomType();
        price = CalculatePrice();
        imageID = 0; //No Images Yet
    }

    int CalculatePrice()
    {
        int calcPrice = Random.Range(20, 38) + Random.Range(20, 39); //Between 40 and 75 (Normalized)
        return calcPrice;
    }

    void RandomStyle()
    {
        styleNumber = Random.Range(1, 4);
        switch (styleNumber)
        {
            case 1:
                style = "Catalan";
                break;
            case 2:
                style = "Frankish";
                break;
            default:
                style = "Venezian";
                break;
        }
    }

    void RandomType()
    {
        typeNumber = Random.Range(1, 6);
        switch (typeNumber)
        {
            case 1:
                type = "Cane"; //Bonus to 'Move Through' Attempts
                break;
            case 2:
                type = "Fan"; //More time to deal with Host Remarks in Conversations (1.5x)
                break;
            case 3:
                type = "Fascinator"; //First Negative Remark in any Conversation is Ignored
                break;
            case 4:
                type = "Garter Flask"; //Increase Max Booze Capacity by 1
                break;
            default:
                type = "Snuff Box"; //Decrease the rate at which you get Intoxicated
                break;
        }
    }

    public string Name()
    {
        return style + " " + type;
    }

    public int Price(string inventory)
    {
        if (inventory == "personal")
        {

            return (int)(price * 0.5);
        }
        else
        {
            return price;
        }
    }

    public string Type()
    {
        return type;
    }

    public int TypeNumber()
    {
        return typeNumber;
    }

    public string Style()
    {
        return style;
    }

    public int StyleNumber()
    {
        return styleNumber;
    }

    public string Description()
    {
        string part1 = "This " + Name() + " ";
        string part2;
        switch (type)
        {
            case "Cane":
                part2 = "helps you delicately push your way through crowds." 
                    + "\n\nAdds a 10% bonus to making 'Move Through' attempts on Rooms.";
                break;
            case "Fan":
                part2 = "adds an air of mystery to you. It helps make you look thoughtful, when you're really just grasping for a witty reply."
                    + "\n\nGives you 50% more time to answer Host Remarks.";
                break;
            case "Fascinator":
                part2 = "distracts people when you might have said the wrong thing. Look how shiny it is!"
                    + "\n\nFirst Negative Remark in any Conversation is Ignored.";
                break;
            case "Garter Flask":
                part2 = "helps you take a little extra drink with you when you refill at the Punch Bowl." 
                    + "\n\nIncreases Max Booze Capacity by 1.";
                break;
            default: //Snuff Box
                part2 = "helps keep you awake and alert, even when you've been overserved by the help."
                    + "\n\nDrinking still gives you Confidence, but doesn't Intoxicate you as much.";
                break;
        }
        return part1 + part2;
    }
}
