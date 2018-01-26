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
			foreach (KeyValuePair<string, ServantVO> servant in servants.Servants)
			{
				model.Livre -= servant.Value.Wage;
			}
		}
	}
}
