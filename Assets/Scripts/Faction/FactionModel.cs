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
		public Dictionary<string, FactionVO> Factions;

		[JsonProperty("factions")]
		private FactionVO[] _factions
		{
			set {
				Factions = new Dictionary<string, FactionVO>();
				foreach(FactionVO faction in value)
				{
					Factions.Add(faction.Name, faction);
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

		public FactionVO this[string faction]
		{
            get
            { return Factions[faction]; }
		}

		public string GetVictoriousPower()
		{
			return Factions[FactionConsts.REVOLUTION].Power > Factions[FactionConsts.CROWN].Power
				? FactionConsts.REVOLUTION
				: FactionConsts.CROWN;
		}

        public FactionVO GetRandomFactionNoNeutral() //Used for a variety of tasks, this returns a randomly selected faction that isn't the 'Neutral' faction used for certain parties
        {
            List<FactionVO> factionList = Factions.Values.ToList();
            List<FactionVO> randomFactionList = new List<FactionVO>();
            foreach (FactionVO f in factionList)
            {
                if(f.Name != "Neutral")
                {
                    randomFactionList.Add(f);
                }
            }
            FactionVO randomFaction = randomFactionList[Util.RNG.Generate(0, randomFactionList.Count)];
            return randomFaction;
        }

		public string GetFactionBenefits(string FactionID)
		{
			string str = "";
			FactionVO faction;
			if (Factions.TryGetValue(FactionID, out faction))
			{
				LocalizationModel phrases = AmbitionApp.GetModel<LocalizationModel>();
				FactionID = FactionID.ToLower().Replace(' ', '_');
				for (int i=faction.Level; i>=0; i--)
				{
					str += phrases.GetString(FactionID + "." + i.ToString());
				}
			}
			return str;
		}
	}
}
