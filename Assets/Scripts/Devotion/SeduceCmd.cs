using System;
using Core;

namespace DeWinter
{
	public class SeduceCmd : ICommand<SeductionVO>
	{
		public void Execute (SeductionVO s)
		{
			NotableVO notable;
			DevotionModel model = DeWinterApp.GetModel<DevotionModel>();
			if (model.Notables.TryGetValue(s.Notable, out notable))
			{
				int seductionChance =
					(notable.Gender == Gender.Male ? model.SeductionModifier : model.SeductionAltModifier) +
					(model.SeductionTimeModifier - s.Time) -
					(string.IsNullOrEmpty(notable.Spouse) ? model.SeductionMarriedModifier : 0);

				if ((new Random()).Next(100) < seductionChance)
				{
					DeWinterApp.SendCommand<DancingCmd, NotableVO>(notable);
				}
			}
		}
	}
}

