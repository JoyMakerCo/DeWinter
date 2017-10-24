using System;
using Core;
using System.Collections.Generic;

namespace Ambition
{
	public class PayWagesCmd : ICommand
	{
		public void Execute ()
		{
			ServantModel servants = AmbitionApp.GetModel<ServantModel>();
			GameModel model = AmbitionApp.GetModel<GameModel>();
			Dictionary<string, ServantVO>.ValueCollection hired = servants.Hired.Values;
			foreach (ServantVO servant in hired)
			{
				model.Livre -= servant.Wage;
			}
		}
	}
}
