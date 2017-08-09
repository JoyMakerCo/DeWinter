using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Ambition
{

	[Serializable]
	public struct RemarkMap
	{
		public string Interest;
		public Sprite InterestSprite;
		public Sprite[] TargetSprites;
	}

	public class PartyArtLibrary : MonoBehaviour
	{
		public RemarkMap[] RemarkSprites;
		public Sprite[] FemaleHostSprites;
		public Sprite[] MaleHostSprites;
	}
}