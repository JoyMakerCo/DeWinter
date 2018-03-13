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
	public struct MomentAudioConfig
	{
		public AudioClip Clip;
		public bool Loop;
		public bool FadeIn;
		public bool FadeOut;
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
		public MomentAudioConfig[] Audio;
		public MomentVO (string text)
		{
			Text = text;
		}
	}
}
