using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ambition
{
	public class RewardVO
	{
		public string Category; // Item, Value, Faction, etc
		public string Type; // Livre, Reputation, etc
		public int Quantity; // Number

		public RewardVO(string category, string type, int quantity=0)
		{
			Category = category;
			Type = type;
			Quantity = quantity;
		}

		// TODO: Localize
		public string Name
		{
			get
			{
				switch (Category)
				{
					case RewardConsts.VALUE:
						return Quantity.ToString() + " " + Type; 
					case RewardConsts.FACTION:
						return Quantity.ToString() + "Repuration with " + Type; 
					case RewardConsts.SERVANT:
						return "An Introduction to Hire a " + Type;
					case RewardConsts.GOSSIP:
						return "A tidbit of " + Type + " Gossip";
				}
				return "";
			}
		}
	}
}