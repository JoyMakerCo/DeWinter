using System;
using Core;
using System.Collections.Generic;

namespace Ambition
{
	public class PayWagesCmd : ICommand
	{
		public void Execute ()
		{
			ServantModel servantModel = AmbitionApp.GetModel<ServantModel>();
			GameModel model = AmbitionApp.GetModel<GameModel>();
			foreach (ServantVO servant in servantModel.Hired.Values)
			{
				model.Livre -= servant.Wage;
			}
		}
	}
}