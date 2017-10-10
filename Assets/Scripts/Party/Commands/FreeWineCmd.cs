using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class FreeWineCmd : ICommand<RoomVO>
	{
		public void Execute(RoomVO room)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
			if(fmod[model.Party.Faction].Level >= 2)
	        {
				AmbitionApp.GetModel<PartyModel>().Drink += model.MaxDrinkAmount;

	            Dictionary<string, string> substitutions = new Dictionary<string, string>()
					{{"$HOSTNAME", model.Party.Host.Name}};
	            AmbitionApp.OpenMessageDialog(DialogConsts.REPUTATION_WINE_DIALOG, substitutions);
	        }
		}
	}
}