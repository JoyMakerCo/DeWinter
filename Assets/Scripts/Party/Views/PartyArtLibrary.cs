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

	[Serializable]
	public struct InterestMap
	{
		public string Interest;
		public Sprite Sprite;
	}

	public class PartyArtLibrary : MonoBehaviour
	{
		public RemarkMap[] RemarkSprites;
		public InterestMap[] InterestSprites;
		public GuestSprite[] MaleGuestSprites;
		public GuestSprite[] FemaleGuestSprites;
		public Sprite[] FemaleHostSprites;
		public Sprite[] MaleHostSprites;
	}
}