using System;
using Core;

namespace Ambition
{
	public class CreateEnemyCmd : ICommand<string>
	{
		public void Execute (string faction)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			EnemyVO e = new EnemyVO();

			// TODO: Find Preset Enemy in model
			e.Faction = faction;
			EnemyInventory.enemyInventory.Add(e);

			Random rnd = new Random();

			e.Like = model.Interests[rnd.Next(model.Interests.Length)];

			e.IsFemale = (rnd.Next(2) == 0);
			if (e.IsFemale)
			{
				e.Name	= rndStr(model.FemaleTitles, rnd) + " "
						+ rndStr(model.FemaleNames, rnd) + " de "
						+ rndStr(model.LastNames, rnd);
			}
			else
			{
				e.Name	= rndStr(model.MaleTitles, rnd) + " "
						+ rndStr(model.MaleNames, rnd) + " de "
						+ rndStr(model.LastNames, rnd);
			}

			e.imageInt = rnd.Next(e.IsFemale ? 4 : 5);
			e.FlavorText = "This person is a great big jerk";
		}

		private string rndStr(string [] strings, Random rnd)
		{
			return strings[rnd.Next(strings.Length)];
		}
	}
}

