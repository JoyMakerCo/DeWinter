using System;
using Core;
using System.Collections.Generic;

namespace DeWinter
{
	public class PayWagesCmd : ICommand
	{
		public void Execute ()
		{
			ServantModel servantModel = DeWinterApp.GetModel<ServantModel>();
			GameModel model = DeWinterApp.GetModel<GameModel>();
			foreach (KeyValuePair<string, ServantVO> kvp in servantModel.Servants)
			{
				model.Livre -= kvp.Value.wage;
			}
		}
	}
}