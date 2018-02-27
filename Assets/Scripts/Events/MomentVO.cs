using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	[Serializable]
	public class MomentVO
	{
		public string Text;
		public Sprite Background;

		public EventRewardVO[] Rewards;

		public MomentVO (string text)
		{
			Text = text;
		}
	}
}
