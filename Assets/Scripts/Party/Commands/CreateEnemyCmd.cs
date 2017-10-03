using System;
using Core;

namespace Ambition
{
	public class CreateEnemyCmd : ICommand<string>
	{
		private LocalizationModel _phrases;

		public void Execute (string faction)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			EnemyVO e = new EnemyVO();
			_phrases = AmbitionApp.GetModel<LocalizationModel>();

			// TODO: Find Preset Enemy in model
			e.Faction = faction;
			EnemyInventory.enemyInventory.Add(e);

			Random rnd = new Random();

			e.Like = model.Interests[rnd.Next(model.Interests.Length)];

			e.IsFemale = (rnd.Next(2) == 0);
			if (e.IsFemale)
			{
				e.Name	= rndStr("female_title", rnd) + " "
						+ rndStr("female_name", rnd) + " de "
						+ rndStr("last_name", rnd);
			}
			else
			{
				e.Name	= rndStr("male_title", rnd) + " "
						+ rndStr("male_name", rnd) + " de "
						+ rndStr("last_name", rnd);
			}

			e.imageInt = rnd.Next(e.IsFemale ? 4 : 5);
			e.FlavorText = "This person is a great big jerk";
		}

		private string rndStr(string phrase, Random rnd)
		{
			string [] list = _phrases.GetList(phrase);
			return list[rnd.Next(list.Length)];
		}
	}
}

