using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ambition
{
	public class FactionVO
	{
		[JsonProperty("Name")]
	    public string Name;

		[JsonProperty("Modesty")]
		public int Modesty; //How modest do they like their outfits?

		[JsonProperty("Luxury")]
		public int Luxury; //How luxurious do they like their outfits?

		[JsonProperty("Steadfast")]
		public bool Steadfast //Would this faction change allegiances?
		{
			get;
			private set;
		}

		[JsonProperty("Baroque")] // "Baroque" is a scale between Rococo (0) and Traditional (100)
		public int[] Baroque;

		[JsonProperty("Allegiance")]
		public int Allegiance; //-100 means the Faction is devoted to the Third Estate, 100 means they are devoted to the Crown

		[JsonProperty("Power")]
		public int Power; //How powerful is this faction in the game?

		public int Level; // Player's Reputation Level with this Faction
	    public int Reputation=0; //What's the Player's Rep with this faction? Raw Number
	    public string knownPower = "Unknown"; //Used in the 'Test the Waters' screen
	    public string knownAllegiance = "Unknown"; //Used in the 'Test the Waters' screen

		public int LargestAllowableParty;
		public int Priority;
        public int DeckBonus=0;
	}
}
