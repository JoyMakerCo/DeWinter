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

			e.Like = model.Interests[Util.RNG.Generate(0, model.Interests.Length)];

			e.Gender = (Util.RNG.Generate(0, 2) == 0) ? Gender.Male : Gender.Female;
			if (e.Gender == Gender.Female)
			{
				e.Title = rndStr("female_title");
				e.FirstName = rndStr("female_name");
			}
			else
			{
				e.Title = rndStr("male_title");
				e.FirstName = rndStr("male_name");
			}

			e.LastName = rndStr("last_name");
			e.imageInt = Util.RNG.Generate(0, (e.Gender == Gender.Female ? 4 : 5));
			e.FlavorText = "This person is a great big jerk";
		}

		private string rndStr(string phrase)
		{
			string [] list = _phrases.GetList(phrase);
			return list[Util.RNG.Generate(0, list.Length)];
		}
	}
}

