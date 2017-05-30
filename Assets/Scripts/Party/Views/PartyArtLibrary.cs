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
		public int ID;
		public Sprite Sprite;
	}

	[Serializable]
	public struct InterestMap
	{
		public string ID;
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