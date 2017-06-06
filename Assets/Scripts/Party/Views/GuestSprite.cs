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
		public Sprite GetSprite(GuestState state)
		{
			int len = GuestSprites.Length;
			switch(state)
			{
				case GuestState.Bored:
					return BoredSprite;
				case GuestState.Charmed:
					return GuestSprites[len-1];
				case GuestState.PutOff:
					return GuestSprites[0];
			}
			return GuestSprites[len > 1 ? 1 : len-1];
		}
	}
}
