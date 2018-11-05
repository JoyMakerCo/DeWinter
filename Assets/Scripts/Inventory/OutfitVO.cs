using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class OutfitVO : ItemVO
	{
        public readonly static OutfitVO Default = new OutfitVO(0, 0, 0, "");

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
            return State.TryGetValue(stat, out result) ? Convert.ToInt32(result) : 0;
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
            GenerateDescription();
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
            result.GenerateName();
            result.GenerateDescription();
            return result;
	    }

		public void CalculatePrice()
	    {
			Price = (int)((Math.Abs(Modesty) + Math.Abs(Luxury))*(float)Novelty*0.01f);
	        if(Style != AmbitionApp.GetModel<InventoryModel>().CurrentStyle) //Check to see if this Outfit matches what's in Style
	        {
				Price = (int)(Price*Ambition.AmbitionApp.GetModel<InventoryModel>().OutOfStyleMultiplier);
	        }
			if (Price < 10) Price = 10; // If the Price is less than 10 make it 10. Will Sell for 5 at most (Sell price is 50% of Buy Price)
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
			return (int)((Util.RNG.Generate(-100,100) + Util.RNG.Generate(-100,100))/2);
            //return (int)Math.Tan(0.0015608f*(Util.RNG.Generate(-1000, 1000))); //The values from this are WAY too normalized. Rarely leave the 2 to -2 range
	    }
	    
	    public void GenerateName()
	    {
            if(Mathf.Abs(Luxury) > Mathf.Abs(Modesty)) //Use the most prominent of the stats for the Outfit name (Otherwise the name is too long and unweildy)
            {
                Name = GenerateLuxuryString() + " " + Style + " " + GenerateNoun();
            } else
            {
                Name = GenerateModestyString() + " " + Style + " " + GenerateNoun();
            }
	    }

        public string GenerateLuxuryString()
        {
            if (Luxury > 60)
            {
                return "Luxurious";
            }
            else if (Luxury > 20)
            {
                return "Pricey";
            }
            else if (Luxury < -60)
            {
                return "Vintage";
            }
            else if (Luxury < -20)
            {
                return "Thrifty";
            } else
            {
                return "Average";
            }
        }

        public string GenerateModestyString()
        {
            if (Modesty > 60)
            {
                return "Virginal";
            }
            else if (Modesty > 20)
            {
                return "Conservative";
            }
            else if (Modesty < -60)
            {
                return "Scandalous";
            }
            else if (Modesty < -20)
            {
                return "Racy";
            }
            else return "Average";
        }

        string GenerateNoun() //This is so the name endings aren't all the same
        {
            string[] nounlist = new string[4];
            nounlist[0] = "Outfit";
            nounlist[1] = "Gown";
            nounlist[2] = "Dress";
            nounlist[3] = "Ensemble";
            return nounlist[UnityEngine.Random.Range(0,nounlist.Length)];
        }

        //To Do: Gifts and such
        void GenerateDescription()
        {
            string luxurytext;
            if (Luxury > 60)
            {
                luxurytext = "A gown of staggering oppulence";
            }
            else if (Luxury > 20)
            {
                luxurytext = "Richly decorated ensemble";
            }
            else if (Luxury < -60)
            {
                luxurytext = "A fine dress for someone with nothing to prove";
            }
            else if (Luxury < -20)
            {
                luxurytext = "Down to earth ensemble";
            }
            else
            {
                luxurytext = "An outfit of decent quality";
            }

            string modestytext;
            if (Modesty > 60)
            {
                modestytext = "modest enough to please the most steadfast tradionalists";
            }
            else if (Modesty > 20)
            {
                modestytext = "also modest and proper";
            }
            else if (Modesty < -60)
            {
                modestytext = "revealing to the point of being scandalous";
            }
            else if (Modesty < -20)
            {
                modestytext = "designed to be provacative";
            }
            else modestytext = "modest, but just provacative enough to be interesting";

            Description = luxurytext + " that's " + modestytext + ".";
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
