using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeWinter
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
	}
}