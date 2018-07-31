using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Ambition
{
    [Serializable]
    public class IncidentVO : DirectedGraph<MomentVO, TransitionVO>
    {
        public string Name;
        public DateTime Date;

        public MomentVO[] Moments;
        public TransitionVO[] Transitions;

        public IncidentVO() {}
        public IncidentVO(DirectedGraph<MomentVO, TransitionVO> incident)
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
        public Vector2[] Positions;
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
