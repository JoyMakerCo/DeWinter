using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ambition
{
	public class Outfit
	{
		[JsonProperty("name")]
	    public string Name;

		[JsonProperty("novelty")]
	    public int novelty;

		[JsonProperty("modesty")]
	    public int modesty;

		[JsonProperty("luxury")]
	    public int luxury;

		[JsonProperty("style")]
	    public string style; // Styles: Frankish, Venezian and Catalan

	    public int price;

	    public bool altered=false; //Each Outfit can only be altered once. This starts as false;

	    // Fully Featured Constructor
	    public Outfit(int nov, int mod, int lux, string sty)
	    {
	        novelty = nov;
	        modesty = mod;
	        luxury = lux;
	        style = sty;
			GenerateName();
	        CalculatePrice();
	    }

	    // Empty/Default Constructor means random outfit
	    public Outfit() {}

	    public static Outfit Create()
	    {
	    	Outfit result = new Outfit();
			result.novelty = 100;
			result.modesty = GenerateRandom();
			result.luxury = GenerateRandom();
			switch (new Random().Next(4))
	        {
	            case 1:
	                result.style = "Frankish";
	                break;
	            case 2:
					result.style = "Venezian";
	                break;
	            default:
					result.style = "Catalan";
	                break;
	        }
			result.CalculatePrice();
			return result;
	    }

	    //Just Style in the string means a randomly generated item of a specific style
	    public Outfit(string sty)
	    {
	        novelty = 100;
	        modesty = GenerateRandom();
	        luxury = GenerateRandom();
	        style = sty;
	        GenerateName();
	        CalculatePrice();
	    }

		public void CalculatePrice(bool sell=false)
	    {
			price = (int)((Math.Abs(modesty) + Math.Abs(luxury))*(float)novelty*0.01f);
	        if(style != AmbitionApp.GetModel<InventoryModel>().CurrentStyle) //Check to see if this Outfit matches what's in Style
	        {
				price = (int)(price*Ambition.AmbitionApp.GetModel<InventoryModel>().OutOfStyleMultiplier);
	        }
			// If the Price is less than 10 make it 10. Will Sell for 5 at most (Sell price is 50% of Buy Price)
			if (price < 10) price = 10;
			if (sell) price = (int)(price*0.5f);
			else
			{
	        	ServantModel smod = AmbitionApp.GetModel<ServantModel>();
				if (smod.Hired.ContainsKey("Seamstress") || smod.Hired.ContainsKey("Tailor"))
				{
					price = (int)(price*smod.SeamstressDiscount);
				}
			}
	    }

	    private static int GenerateRandom()
	    {
			return (int)Math.Tan(0.0015608f*(new Random().Next(-1000, 1000)));
	    }
	    
	    private void GenerateName()
	    {
	        string luxuryString=null;
	        //Modesty Conversion
	        if (modesty > 60)
	        {
	            Name = "Virginal";
	        }
			else if (modesty > 20)
	        {
				Name = "Conservative";
	        }
			else if (modesty < -60)
	        {
				Name = "Scandalous";
	        }
			else if (modesty < -20)
	        {
				Name = "Racy";
	        }
	        else Name = null;

	        //Luxury Conversion
	        if (luxury > 60)
	        {
	            luxuryString = "Luxurious";
	        }
			else if (luxury > 20)
	        {
	            luxuryString = "Pricey";
	        }
			else if (luxury < -60)
	        {
				luxuryString = "Vintage";
	        }
			else if (luxury < -20)
	        {
				luxuryString = "Thrifty";
	        }

	        //Return Dat Shit!
	        if (luxuryString != null)
	        {
				Name = (Name != null) ? (Name + ", " + luxuryString) : luxuryString;
	        }
			Name = (Name + " " + style + " Outfit") ?? (style + " Outfit");
	    }

	    public void Alter(string stat, int amount)
	    {
	        if (!altered)
	        {
	            if (stat == "Luxury")
	            {
	                luxury += amount;
	                if (luxury < -100) luxury = -100;
	                if (luxury > 100) luxury = 100;
	            }
	            else if (stat == "Modesty")
	            {
					modesty += amount;
					if (modesty < -100) modesty = -100;
					if (modesty > 100) modesty = 100;
	            }
	            novelty += 10;
	            if (novelty > 100) novelty = 100;
	            altered = true;
	        }
	    }
	}
}
