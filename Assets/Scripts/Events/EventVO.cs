using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ambition
{
	[Serializable]
	public class EventVO
	{
		public string Name;
		public EventSetting Setting;

		public MomentVO[] Moments;
		public EventConfigLinkVO[] Links;

		#if (UNITY_EDITOR)
		public Vector2[] Positions;
		#endif
	}

	[Serializable]
	public struct EventConfigLinkVO
	{
		public int Index;
		public int Target;
		public string Text;
	}
}
