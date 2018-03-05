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

			e.Gender = (rnd.Next(2) == 0) ? Gender.Male : Gender.Female;
			if (e.Gender == Gender.Female)
			{
				e.Title = rndStr("female_title", rnd);
				e.FirstName = rndStr("female_name", rnd);
			}
			else
			{
				e.Title = rndStr("male_title", rnd);
				e.FirstName = rndStr("male_name", rnd);
			}

			e.LastName = rndStr("last_name", rnd);
			e.imageInt = rnd.Next(e.Gender == Gender.Female ? 4 : 5);
			e.FlavorText = "This person is a great big jerk";
		}

		private string rndStr(string phrase, Random rnd)
		{
			string [] list = _phrases.GetList(phrase);
			return list[rnd.Next(list.Length)];
		}
	}
}

