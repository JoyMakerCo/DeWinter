using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Util;
using UGraph;

namespace Ambition
{
    [Serializable]
    public class IncidentVO : DirectedGraph<MomentVO, TransitionVO>
    {
        // PUBLIC DATA //////////////////

        public string ID { get; set; }
        public bool OneShot = true;
        public int[] Chapters;
        public FactionType[] Factions;
        public bool IsScheduled => Date != default;
        public bool Political = false;
        public DateTime Date
        {
            get => new DateTime(_date);
            set => _date = value.Ticks;
        }
        public RequirementVO[] Requirements;

        // PRIVATE/PROTECTED DATA //////////////////
        [SerializeField] private long _date = 0;

        // CONSTRUCTOR //////////////////

        public IncidentVO() : base() { }
        public IncidentVO(DirectedGraph<MomentVO, TransitionVO> graph) : base(graph) { }
        public IncidentVO(IncidentVO incident) : base(incident as DirectedGraph<MomentVO, TransitionVO>)
        {
            this.ID = incident.ID;
            this._date = incident._date < 0 ? 0 : incident._date;
            this.OneShot = incident.OneShot;
            this.ID = incident.ID;
            this.Political = incident.Political;
        }

        // PUBLIC METHODS //////////////////

        public string [] GetCharacters()
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

        public string[] Dump()
        {
            //return new string[] { "incident " + Name };

            var chapz = (Chapters == null) ? "null" : (string.Join(", ", Chapters.Select( x => x.ToString() ).ToArray() ));
            var fax = Factions == null ? "null" : string.Join(", ", Factions.Select( x => x.ToString() ).ToArray() );
            var charz = string.Join(", ", GetCharacters() );
            return new string[] 
            {
                "Incident: "+ID+":",
                "characters "+charz,
                "chapters "+ chapz,
                "factions "+ fax,
                "scheduled "+ (IsScheduled ? Date.ToString() : "false"),
                "oneshot "+ (OneShot ? "true" : "false"),
            };

            // TODO dump requirements, characters...?
        }

        public override string ToString()
        {
            return "Incident: "+ID;
        }
    }
}
