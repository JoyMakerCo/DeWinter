using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;
using UnityEngine;

namespace Ambition
{
    [Saveable]
	public class FactionModel : ObservableModel<FactionModel>, IResettable
	{
        [JsonIgnore]
        public Dictionary<FactionType, FactionVO> Factions = new Dictionary<FactionType, FactionVO>();

		[JsonProperty("factions")]
		private FactionStandingsVO[] _factions
		{
			set
            {
                FactionVO faction;
                foreach (FactionStandingsVO standings in value)
                {
                    if (Factions.TryGetValue(standings.Faction, out faction))
                    {
                        faction.Allegiance = standings.Allegiance;
                        faction.Power = standings.Power;
                    }
                }
            }
            get
            {
                List<FactionStandingsVO> result = new List<FactionStandingsVO>();
                FactionStandingsVO standings;
                foreach (FactionVO faction in Factions.Values)
                {
                    standings = new FactionStandingsVO()
                    {
                        Faction = faction.Type,
                        Allegiance = faction.Allegiance,
                        Power = faction.Power
                    };
                    result.Add(standings);
                }
                return result.ToArray();
            }
        }

        [JsonProperty("lastupdated")]
        public int LastUpdated;

        [JsonProperty("standings")]
        public List<FactionStandingsVO> Standings = new List<FactionStandingsVO>();

        [JsonIgnore]
        public FactionVO this[FactionType faction] => Factions[faction];

        [JsonIgnore]
        public List<FactionStandingsVO> ResetValues = new List<FactionStandingsVO>();

        public void Reset()
        {
            FactionVO faction;
            foreach(FactionStandingsVO standings in ResetValues)
            {
                Factions.TryGetValue(standings.Faction, out faction);
                if (faction != null)
                {
                    faction.Allegiance = standings.Allegiance;
                    faction.Power = standings.Power;
                }
            }
        }

        public override string ToString()
        {
            string result = "FactionModel:\n";
            foreach (FactionVO faction in Factions.Values)
            {
                result += faction.ToString();
            }
            return result;
        }
	}
}
