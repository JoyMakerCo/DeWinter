using System;

namespace Ambition
{
	public class AdjustFactionVO
	{
		public string Faction;

		public int Allegiance;
		public int Reputation;
		public int Power;

		public AdjustFactionVO (string faction, int reputation, int power=0, int allegiance=0)
		{
			Faction=faction;
			Reputation=reputation;
			Power=power;
			Allegiance=allegiance;
		}

		public static AdjustFactionVO MakeReputationVO(string faction, int reputation)
		{
			return new AdjustFactionVO(faction, reputation);
		}

		public static AdjustFactionVO MakePowerVO(string faction, int power)
		{
			return new AdjustFactionVO(faction, 0, power);
		}

		public static AdjustFactionVO MakeAllegianceVO(string faction, int allegiance)
		{
			return new AdjustFactionVO(faction, 0, 0, allegiance);
		}

	}
}