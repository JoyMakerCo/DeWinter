using System;

namespace Ambition
{
	public class ModifierVO
	{
		public string ID;
		public string Type;
		public float Multiplier=1.0f;
		public float Bonus=0.0f;

		public ModifierVO () {}
		public ModifierVO (string id, string type, float multiplier, float bonus)
		{
			ID = id;
			Type = type;
			Multiplier = multiplier;
			Bonus = bonus;
		}
	}
}
