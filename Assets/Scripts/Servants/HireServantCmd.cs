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

			// Block if the position is filled
			if (!model.Servants.ContainsKey(servant.Slot))
			{
				List<ServantVO> servants;
				model.Servants.Add(servant.Slot, servant);
				servant.Status = ServantStatus.Hired;
				if (model.Applicants.TryGetValue(servant.Slot, out servants))
					servants.Remove(servant);
				if (model.Unknown.TryGetValue(servant.Slot, out servants))
					servants.Remove(servant);
				AmbitionApp.GetModel<GameModel>().Livre.Value -= servant.Wage;
			}
		}
	}
}
