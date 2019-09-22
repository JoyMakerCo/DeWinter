using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;
using UnityEngine;

namespace Ambition
{
	public class FactionModel : DocumentModel
	{
		public Dictionary<FactionType, FactionVO> Factions;

		[JsonProperty("factions")]
		private FactionVO[] _factions
		{
			set {
				Factions = new Dictionary<FactionType, FactionVO>();
				foreach(FactionVO faction in value)
				{
					Factions.Add(faction.Type, faction);
				}
			}
		}

		[JsonProperty("levels")]
		public FactionLevelVO[] Levels;

		[JsonProperty("power")]
		public int [] Power;

		[JsonProperty("modesty")]
		public int [] Modesty;

		[JsonProperty("luxury")]
		public int [] Luxury;

		[JsonProperty("allegiance")]
		public int [] Allegiance;

		public FactionModel () : base("FactionData") {}

		public FactionVO this[FactionType faction] => Factions[faction];

		public FactionType GetVictoriousPower()
		{
			return Factions[FactionType.Revolution].Power > Factions[FactionType.Crown].Power
				? FactionType.Revolution
				: FactionType.Crown;
		}

        public FactionVO GetRandomFactionNoNeutral() //Used for a variety of tasks, this returns a randomly selected faction that isn't the 'Neutral' faction used for certain parties
        {
            FactionVO[] factions = Factions.Values.Where(f => f.Type != FactionType.Neutral).ToArray();
            return Util.RNG.TakeRandom(factions);
        }

		public string GetFactionBenefits(FactionType type)
		{
            if (!Factions.TryGetValue(type, out FactionVO faction)) return "";
            int level = faction.Level;
            string[] results = new string[level];
			for (int i=level; i>=0; i--)
			{
                results[i] = AmbitionApp.GetString(faction.Name.ToLower() + "." + (level - i).ToString());
            }
            return string.Join("\n", results);
        }

		public FactionLevelVO GetFactionLevel( int level )
		{
			if (level < 0)
			{
				Debug.LogErrorFormat("Faction level is {0} ???",level);
				level = 0;
			}

			if (level >= Levels.Length)
			{
				Debug.LogErrorFormat("Faction level is {0} (max {1}) ???",level,Levels.Length-1);
				level = Levels.Length-1;
			}

			return Levels[level];
		}
	}
}
