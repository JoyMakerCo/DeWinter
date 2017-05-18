using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class PartyArtLibrary : MonoBehaviour
	{
		public KeyValuePair<int, Sprite>[] RemarkSprites;
		public KeyValuePair<string, Sprite>[] TopicSprites;
		public GuestSprite[] MaleGuestSprites;
		public GuestSprite[] FemaleGuestSprites;
		public Sprite[] FemaleHostSprites;
		public Sprite[] MaleHostSprites;
	}
}