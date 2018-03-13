using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
	[Serializable]
	public class IncidentVO
	{
		public string Name;
		public IncidentSetting Setting;

		public MomentVO[] Moments;
		public TransitionVO[] Transitions;

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
