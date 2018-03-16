using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
	[Serializable]
	public struct IncidentCharacterConfig
	{
		public string AvatarID;
		public string Pose;
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
		public AmbientClip Music;
		public AudioClip[] AudioClips;
		public MomentVO (string text)
		{
			Text = text;
		}
	}
}
