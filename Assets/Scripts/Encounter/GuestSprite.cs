using System;
using UnityEngine;

namespace DeWinter
{
	[Serializable]
	public class GuestSprite
	{
		public string ID;
		public Sprite[] StateSprites;

		// Returns a Sprite corresponding to the guest's approval
		public Sprite GetSpite(int Opinion)
		{
			float percent = 0.1f*Mathf.Clamp((float)Opinion, 0f, 99.0f);
			return StateSprites[(int)(percent*(float)StateSprites.Length)];
		}
	}
}