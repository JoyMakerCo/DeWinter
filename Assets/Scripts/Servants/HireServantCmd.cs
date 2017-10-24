using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class HireServantCmd : ICommand<ServantVO>
	{
		public void Execute (ServantVO servant)
		{
			ServantModel model = AmbitionApp.GetModel<ServantModel>();
			servant = Array.Find(model.Servants, s=>s.ID == servant.ID);
			if (servant != null)
			{
				servant.Introduced = servant.Hired = true;
				if (!model.Introduced.Contains(servant)) model.Introduced.Add(servant);
				model.Hired[servant.Slot] = servant;
				AmbitionApp.GetModel<GameModel>().Livre -= servant.Wage;
			}
		}
	}
}
