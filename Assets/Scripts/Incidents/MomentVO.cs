﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	[Serializable]
	public struct IncidentCharacterConfig
	{
		public AvatarVO Avatar;
		public string Name;
	}

	[Serializable]
	public class MomentVO
	{
		public string Text;
		public Sprite Background;
		public IncidentCharacterConfig Character1;
		public IncidentCharacterConfig Character2;
		public SpeakerType Speaker;
		public CommodityVO[] Rewards;

		public MomentVO (string text)
		{
			Text = text;
		}
	}
}
