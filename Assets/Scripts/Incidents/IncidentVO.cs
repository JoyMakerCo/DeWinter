﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Util;
using UGraph;
using Newtonsoft.Json;

namespace Ambition
{
    [Serializable]
    public class IncidentVO : DirectedGraph<MomentVO, TransitionVO>, ICalendarEvent
    {
        public string Name { get; set; }

        public string AssetPath;

        [JsonProperty("one_shot")]
        public bool OneShot = true;

        public int[] Chapters;
        public string[] Tags;
        public FactionType[] Factions;

        [JsonProperty("localization_key")]
        public string LocalizationKey;

        [JsonIgnore]
        public bool IsScheduled => Date > default(DateTime);

        [JsonProperty("complete")]
        public bool IsComplete { set; get; }

        [JsonIgnore]
        public DateTime Date { set; get; }

        public RequirementVO[] Requirements;

        public /*CharactetVO*/string [] GetCharacters()
        {
            if (Nodes == null) return null;
            List<string> result = new List<string>();
            string name;
            foreach(MomentVO moment in Nodes)
            {
                name = moment.Character1.Name;
                if (!string.IsNullOrWhiteSpace(name) && !result.Contains(name)) result.Add(name);
                name = moment.Character2.Name;
                if (!string.IsNullOrWhiteSpace(name) && !result.Contains(name)) result.Add(name);
            }
            return result.ToArray();
        }

        private string CharName(IncidentCharacterConfig config)
        {
            return string.IsNullOrWhiteSpace(config.Name) ? null : config.Name;
        }

        public IncidentVO() : base() {}
        public IncidentVO(DirectedGraph<MomentVO, TransitionVO> graph) : base(graph) { }

        public IncidentVO(IncidentVO incident) : base(incident as DirectedGraph<MomentVO, TransitionVO>)
        {
            this.Name = incident.Name;
            this.Date = incident.Date;
            this.OneShot = incident.OneShot;
        }
    }

    [Serializable]
    public class TransitionVO
    {
        public string Text;
        public bool xor=false;
        public CommodityVO[] Rewards;
        public RequirementVO[] Requirements;
        public IncidentFlag[] Flags;

        public override string ToString()
        {
			return string.Format( "TransitionVO: {0} {1} rewards, {2} requirements", Text.Truncate( 16 ), Rewards.Length, Requirements.Length );
        }
    }
}
