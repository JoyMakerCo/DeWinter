using System;

namespace Ambition
{
	public class AdjustFactionVO
	{
		public FactionType Faction;

		public int Allegiance;
		public int Reputation;
		public int Power;

		public AdjustFactionVO (FactionType faction, int reputation, int power=0, int allegiance=0)
		{
			Faction=faction;
			Reputation=reputation;
			Power=power;
			Allegiance=allegiance;
		}

		public static AdjustFactionVO MakeReputationVO(FactionType faction, int reputation)
		{
			return new AdjustFactionVO(faction, reputation);
		}

		public static AdjustFactionVO MakePowerVO(FactionType faction, int power)
		{
			return new AdjustFactionVO(faction, 0, power);
		}

		public static AdjustFactionVO MakeAllegianceVO(FactionType faction, int allegiance)
		{
			return new AdjustFactionVO(faction, 0, 0, allegiance);
		}

		public override string ToString()
		{
			return string.Format( "AdjustFactionVO {0} A {1:+0;-#} R {2:+0;-#} P {3:+0;-#}", Faction, Allegiance, Reputation, Power);
		}
	}
}