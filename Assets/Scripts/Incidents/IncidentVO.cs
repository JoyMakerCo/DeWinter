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
        public bool Active = false;
        public DateTime Date;

		public MomentVO[] Moments;
		public TransitionVO[] Transitions;

        public IncidentVO() {}
        public IncidentVO(DirectedGraph<MomentVO, TransitionVO> graph)
        {
            this.Moments = graph.Nodes;
            this.Links = graph.Links;
            this.LinkData = graph.LinkData;
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
		public CommodityVO [] Rewards;
	}
}
