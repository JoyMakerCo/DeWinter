using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class PartyArtLibrary : MonoBehaviour
	{
		public KeyValuePair<int, Sprite>[] RemarkSprites;
		public KeyValuePair<string, Sprite>[] TopicSprites;
		public KeyValuePair<int, GuestSprite>[] GuestSprites;
		public KeyValuePair<string, Sprite> HostSprites;
	}
}