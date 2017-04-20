using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

public class Outfit
{
    public int novelty;
    public int modesty;
    public int luxury;
    public string style; // Styles: Frankish, Venezian and Catalan
    public int price;
    public int imageID;
    public bool altered; //Each Outfit can only be altered once. This starts as false;

    // Fully Featured Constructor
    public Outfit(int nov, int mod, int lux, string sty)
    {
        novelty = nov;
        modesty = mod;
        luxury = lux;
        style = sty;
        price = CalculatePrice();
        imageID = 1;
        altered = false;
    }

    // Empty/Default Constructor means random outfit
    public Outfit()
    {
        novelty = 100;
        modesty = GenerateRandom();
        luxury = GenerateRandom();
        int styleNumber = Random.Range(1, 4);
        switch (styleNumber)
        {
            case 1:
                style = "Frankish";
                break;
            case 2:
                style = "Venezian";
                break;
            default:
                style = "Catalan";
                break;
        }
        price = CalculatePrice();
        imageID = styleNumber;
        altered = false;
    }

    //Just Style in the string means a randomly generated item of a specific style
    public Outfit(string sty)
    {
        novelty = 100;
        modesty = GenerateRandom();
        luxury = GenerateRandom();
        style = sty;
        price = CalculatePrice();
        imageID = 1;
    }

    public void PrintValues()
    {
        Debug.Log("Novelty: " + novelty + ", Modesty: " + modesty + ", Luxury: " + luxury + ", Style : " + style);
    }

    public void UpdatePrice()
    {
        price = CalculatePrice();
    }

    int CalculatePrice()
    {
        float noveltyPercent = (float)novelty / 100;
        int calcPrice = (int)((Mathf.Abs(modesty) + Mathf.Abs(luxury))*noveltyPercent);
        if(style != DeWinterApp.GetModel<InventoryModel>().CurrentStyle) //Check to see if this Outfit matches what's in Style
        {
// TODO: Populated by model
            calcPrice = (int)(calcPrice*DeWinter.DeWinterApp.GetModel<InventoryModel>().OutOfStyleMultiplier);
        }
        if (calcPrice < 10) // If the Price is less than 10 make it 10. Will Sell for 5 at most (Sell price is 50% of Buy Price)
        {
            calcPrice = 10;
        }
        return calcPrice;
    }

    public int OutfitPrice(string inventory)
    {
		float outfitPrice = (float)CalculatePrice();
        if(inventory == "personal")
        {
            return (int)(outfitPrice * 0.5f);
        } else
        {
// TODO: This value should be modified within the model, not the VO
        	ServantModel smod = DeWinterApp.GetModel<ServantModel>();
			if (smod.Hired.ContainsKey("Seamstress") || smod.Hired.ContainsKey("Tailor"))
			{
				return (int)(outfitPrice * smod.SeamstressDiscount);
			}
			return (int)outfitPrice;
        }
    }

    int GenerateRandom()
    {
		return (int)Mathf.Tan(Random.Range(-1.5608f, 1.5608f));
    }
    
    public string Name()
    {
        string name;
        name = numberToTextConversion(modesty, luxury) + " " + style + " Outfit";
        return name;
    }

    string numberToTextConversion(int mod, int lux)
    {
        string modestyString = null;
        string luxuryString = null;
        //Modesty Conversion
        if (mod > 60)
        {
            modestyString = "Virginal";
        }
        else if (mod > 20 && mod <= 60)
        {
            modestyString = "Conservative";
        }
        else if (mod >= -20 && mod <= 20)
        {
            modestyString = "Average";
        }
        else if (mod >= -60 && mod < -20)
        {
            modestyString = "Racy";
        }
        else if (mod < -60)
        {
            modestyString = "Scandalous";
        }
        //Luxury Conversion
        if (lux > 60)
        {
            luxuryString = "Luxurious";
        }
        else if (lux > 20 && lux <= 60)
        {
            luxuryString = "Pricey";
        }
        else if (lux >= -20 && lux <= 20)
        {
            luxuryString = "Average";
        }
        else if (lux >= -60 && lux < -20)
        {
            luxuryString = "Thrifty";
        }
        else if (lux < -60)
        {
            luxuryString = "Vintage";
        }
        //Return Dat Shit!
        if (luxuryString == modestyString)
        {
            return "Average"; //Because "Average Average Outfit" doesn't make sense
        }
        else
        {
            return luxuryString + ", " + modestyString;
        }
    }

    public void Alter(string stat, int amount)
    {
        if (!altered)
        {
            if (stat == "Luxury")
            {
                luxury = Mathf.Clamp(luxury + amount, -100, 100);
                Debug.Log("Luxury Altered by " + amount + ". Final Luxury is " + (luxury + amount) + " or " + luxury);
            }
            else if (stat == "Modesty")
            {
                modesty = Mathf.Clamp(modesty + amount, -100, 100);
            }
            else
            {
                Debug.Log("Something went wrong with the Alter command for this Outfit. Did you enter the right stat?");
            }
            novelty = Mathf.Clamp(novelty + 10, 10, 100);
            altered = true;
        } else
        {
            Debug.Log("Can't Alter an Outfit more than once!");
        }
    }
}
