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
        public FMODEvent Music;
        public FMODEvent AmbientSFX;
        public FMODEvent OneShotSFX; // <- This handles both actual SFX (knocking on doors, etc...) and musical stings that get used once (Shocked, Reveal: Intrigue, etc...)
	}
}
