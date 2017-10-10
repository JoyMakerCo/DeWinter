using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class OutfitVO : ItemVO
	{
		public OutfitVO() {}
		public OutfitVO(ItemVO outfit) : base(outfit) {}

	    public int Novelty
	    {
			get { return GetStat(InventoryConsts.NOVELTY); }
    		set { States[InventoryConsts.NOVELTY] = value; }
		}
	    
		public int Modesty
	    {
			get { return GetStat(InventoryConsts.MODESTY); }
    		set { States[InventoryConsts.MODESTY] = value; }
		}

		public int Luxury
	    {
			get { return GetStat(InventoryConsts.LUXURY); }
    		set { States[InventoryConsts.LUXURY] = value; }
		}	    

		public string Style
	    {
			get {
				object result;
				States.TryGetValue(InventoryConsts.STYLE, out result);
				return (string)result;
			}
    		set { States[InventoryConsts.STYLE] = value; }
		}

	    private int GetStat(string stat)
	    {
			object result;
    		return States.TryGetValue(InventoryConsts.NOVELTY, out result) ? (int)result : 0;
	    }

	    public bool Altered
	    {
			get { return Tags.Contains(InventoryConsts.ALTERED); }
	    }

	    // Fully Featured Constructor
	    public OutfitVO(int nov, int mod, int lux, string sty)
	    {
	        Novelty = nov;
	        Modesty = mod;
	        Luxury = lux;
	        Style = sty;
			GenerateName();
	        CalculatePrice();
	    }

	    // Empty/Default Constructor means random outfit
	    public static OutfitVO Create()
	    {
	    	OutfitVO result = new OutfitVO();
			result.Novelty = 100;
			result.Modesty = GenerateRandom();
			result.Luxury = GenerateRandom();
			switch (new Random().Next(4))
	        {
	            case 1:
	                result.Style = "Frankish";
	                break;
	            case 2:
					result.Style = "Venezian";
	                break;
	            default:
					result.Style = "Catalan";
	                break;
	        }
			result.CalculatePrice();
			return result;
	    }

	    //Just Style in the string means a randomly generated item of a specific style
	    public OutfitVO(string sty)
	    {
	        Novelty = 100;
	        Modesty = GenerateRandom();
	        Luxury = GenerateRandom();
	        Style = sty;
	        GenerateName();
	        CalculatePrice();
	    }

		public void CalculatePrice(bool sell=false)
	    {
			Price = (int)((Math.Abs(Modesty) + Math.Abs(Luxury))*(float)Novelty*0.01f);
	        if(Style != AmbitionApp.GetModel<InventoryModel>().CurrentStyle) //Check to see if this Outfit matches what's in Style
	        {
				Price = (int)(Price*Ambition.AmbitionApp.GetModel<InventoryModel>().OutOfStyleMultiplier);
	        }
			// If the Price is less than 10 make it 10. Will Sell for 5 at most (Sell price is 50% of Buy Price)
			if (Price < 10) Price = 10;
			if (sell) Price = (int)(Price*0.5f);
			else
			{
	        	ServantModel smod = AmbitionApp.GetModel<ServantModel>();
				if (smod.Hired.ContainsKey("Seamstress") || smod.Hired.ContainsKey("Tailor"))
				{
					Price = (int)(Price*smod.SeamstressDiscount);
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
	        if (Modesty > 60)
	        {
	            Name = "Virginal";
	        }
			else if (Modesty > 20)
	        {
				Name = "Conservative";
	        }
			else if (Modesty < -60)
	        {
				Name = "Scandalous";
	        }
			else if (Modesty < -20)
	        {
				Name = "Racy";
	        }
	        else Name = null;

	        //Luxury Conversion
	        if (Luxury > 60)
	        {
	            luxuryString = "Luxurious";
	        }
			else if (Luxury > 20)
	        {
	            luxuryString = "Pricey";
	        }
			else if (Luxury < -60)
	        {
				luxuryString = "Vintage";
	        }
			else if (Luxury < -20)
	        {
				luxuryString = "Thrifty";
	        }

	        //Return Dat Shit!
	        if (luxuryString != null)
	        {
				Name = (Name != null) ? (Name + ", " + luxuryString) : luxuryString;
	        }
			Name = (Name + " " + Style + " Outfit") ?? (Style + " Outfit");
	    }

	    public void Alter(string stat, int amount)
	    {
	        if (!Altered)
	        {
	        	amount += GetStat(stat);
				if (amount < -100) amount = 100;
				else if (amount > 100) amount = 100;
				States[stat] = amount;
	            Tags.Add(InventoryConsts.ALTERED);
	        }
	    }
	}
}
