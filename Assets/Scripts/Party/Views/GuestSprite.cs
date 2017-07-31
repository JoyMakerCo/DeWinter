using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	[Serializable]
	public class GuestSprite
	{
		public Sprite BoredSprite;

		public Sprite[] GuestSprites;

		// Returns a Sprite corresponding to the guest's approval
		// TODO: This may eventually be completely handled by Mecanim
		public Sprite GetSprite(GuestVO guest)
		{
			int len = GuestSprites.Length;
			if (guest.State == GuestState.Bored)
				return BoredSprite;
			if (guest.Opinion <= 1)
				return GuestSprites[0];
			if (guest.Opinion >= 100)
				return GuestSprites[GuestSprites.Length-1];
			return GuestSprites[1 + (int)((GuestSprites.Length-2)*(float)(guest.Opinion)*0.01f)];
		}
	}
}
