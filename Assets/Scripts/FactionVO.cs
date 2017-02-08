using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeWinter
{
	internal struct FactionLevel
	{
		[JsonProperty("requirement")]
		public int Requirement;

		[JsonProperty("text")]
		public string Text;

		[JsonProperty("confidence")]
		public int Confidence;

		[JsonProperty("importance")]
		public int Importance;
	}

	public class FactionVO
	{
		private float _power;
		private float _allegiance;

		[JsonProperty("Name")]
	    public string Name;

		[JsonProperty("Modesty")]
		public int Modesty; //How modest do they like their outfits?

		[JsonProperty("Luxury")]
		public int Luxury; //How luxurious do they like their outfits?

		[JsonProperty("Steadfast")]
		private bool _steadfast; //Would this faction change allegiances?

		[JsonProperty("Allegiance")]
		public float Allegiance //-100 means the Faction is devoted to the Revolution, 100 means they are devoted to the Crown
		{
			get { return _allegiance; }
			set
			{
				if (!_steadfast)
				{
					_allegiance = Mathf.Clamp(value, -100f, 100f);
				}
			}
		}

		[JsonProperty("Power")]
		public float Power //How powerful is this faction in the game?
		{
			get { return _power; }
			set { _power = Mathf.Clamp(value, 0f, 100f); }
		}

		[JsonProperty("Levels")]
		private FactionLevel[] _levels;

		public int RepLevel
		{
			get
			{
				for (int i=_levels.Length-1; i>=0; i--)
				{
					if (playerReputation >= _levels[i].Requirement)
						return i;
				}
				return 0;
			}
		}

	    public int playerReputation=0; //What's the Player's Rep with this faction? Raw Number
	    public string knownPower = "Unknown"; //Used in the 'Test the Waters' screen
	    public string knownAllegiance = "Unknown"; //Used in the 'Test the Waters' screen

		public int ConfidenceBonus
		{
			get { return _levels[RepLevel].Confidence; }
		}

	// TODO: This belongs in the View, not in the data.
		public int powerKnowledgeTimer;
	    public int allegianceKnowledgeTimer;
	}
}
/*
TODO: Display strings; should be used in Views, not data
likes = "They don't care about your clothes";
likes = ll + " and " + ml + " Outfits"
*/