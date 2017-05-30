using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class GuestSprite
	{
		public Sprite PutOffSprite;
		public Sprite BoredSprite;
		public Sprite InterestedSprite;
		public Sprite CharmedSprite;

		// Returns a Sprite corresponding to the guest's approval
		// TODO: This may eventually be completely handled by Mecanim
		public Sprite GetSprite(GuestState state)
		{
			switch(state)
			{
				case GuestState.Bored:
					return BoredSprite;
				case GuestState.Charmed:
					return CharmedSprite;
				case GuestState.PutOff:
					return PutOffSprite;
			}
			return InterestedSprite;
		}
	}
}