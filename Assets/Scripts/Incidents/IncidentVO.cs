using System;
using UnityEngine;
using Util;

namespace Ambition
{
    [Serializable]
    public class IncidentVO : DirectedGraph<MomentVO, TransitionVO>, ICalendarEvent
    {
        public string Name;

        [SerializeField]
        private long _date = -1;

        public bool IsScheduled
        {
            get { return _date > 0; }
        }

        public DateTime Date
        {
            get { return _date > 0 ? DateTime.MinValue.AddTicks(_date) : DateTime.MinValue; }
            set { _date = value.Ticks; }
        }

        public CommodityVO[] Requirements;

        public bool OneShot = true;

        public IncidentVO() : base() {}
        public IncidentVO(DirectedGraph<MomentVO, TransitionVO> incident) : base(incident)
        {
            Nodes = incident.Nodes;
            Links = incident.Links;
            LinkData = incident.LinkData;
        }

        public IncidentVO(IncidentVO incident) : this(incident as DirectedGraph<MomentVO, TransitionVO>)
        {
            this.Name = incident.Name;
            this.Date = incident.Date;
        }
#if (UNITY_EDITOR)
        public int Month = 0;
        public int Day = 0;
        public int Year = 0;
#endif
    }

    [Serializable]
    public class TransitionVO
    {
        public int Index;
        public int Target;
        public string Text;
        public CommodityVO[] Rewards;
    }
}
