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

        public string ID;
        public bool OneShot = true;
        public int[] Chapters;
        public IncidentType Type;
        public FactionType[] Factions;
        public bool IsScheduled => _date > 0;
        public bool Political = false;
        public string Description = "";
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
            this.Requirements = incident.Requirements;
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

        public override string ToString()
        {
            var chapz = (Chapters == null) ? "null" : (string.Join(", ", Chapters.Select( x => x.ToString() ).ToArray() ));
            var fax = Factions == null ? "null" : string.Join(", ", Factions.Select( x => x.ToString() ).ToArray() );
            return "Incident: " + ID +
                "\n characters " + string.Join(", ", GetCharacters()) +
                "\n chapters " + chapz +
                "\n factions " + fax +
                "\n scheduled " + (IsScheduled ? Date.ToString() : "false") +
                "\n oneshot " + (OneShot ? "true" : "false");
        }
    }
}
