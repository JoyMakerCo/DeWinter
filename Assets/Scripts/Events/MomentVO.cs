using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	[Serializable]
	public struct EventCharacterConfig
	{
		public AvatarVO Avatar;
		public string Name;
	}

	[Serializable]
	public class MomentVO
	{
		public string Text;
		public Sprite Background;
		public EventCharacterConfig Character1;
		public EventCharacterConfig Character2;
		public CommodityVO[] Rewards;

		public MomentVO (string text)
		{
			Text = text;
		}
	}
}
