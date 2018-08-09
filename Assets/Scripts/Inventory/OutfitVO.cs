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
			get { return GetStat(ItemConsts.NOVELTY); }
    		set { State[ItemConsts.NOVELTY] = value.ToString(); }
		}
	    
		public int Modesty
	    {
			get { return GetStat(ItemConsts.MODESTY); }
    		set { State[ItemConsts.MODESTY] = value.ToString(); }
		}

		public int Luxury
	    {
			get { return GetStat(ItemConsts.LUXURY); }
    		set { State[ItemConsts.LUXURY] = value.ToString(); }
		}	    

		public string Style
	    {
			get {
				object result;
				return State.TryGetValue(ItemConsts.STYLE, out result) ? (string)result : null;
			}
    		set { State[ItemConsts.STYLE] = value; }
		}

	    private int GetStat(string stat)
	    {
			object result;
			return State.TryGetValue(ItemConsts.NOVELTY, out result) ? Convert.ToInt32(result) : 0;
	    }

	    public bool Altered
	    {
			get { return Tags.Contains(ItemConsts.ALTERED); }
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
			switch (Util.RNG.Generate(0,4))
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
				InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
				ItemVO item = inventory.Inventory.Find(i=>i.State.ContainsKey("outfit_cost"));
				if (item != null)
				{
					Price = (int)(Price*Convert.ToDouble(item.State["outfit_cost"]));
				}
			}
	    }

	    private static int GenerateRandom()
	    {
			return (int)Math.Tan(0.0015608f*(Util.RNG.Generate(-1000, 1000)));
	    }
	    
	    public void GenerateName()
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
				State[stat] = amount.ToString();
	            Tags.Add(ItemConsts.ALTERED);
	        }
	    }
	}
}
