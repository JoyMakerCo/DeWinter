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
			if (servant == null || !servant.Introduced)
			{
				Debug.Log("Can't Hire a Servant who hasn't been introduced yet");
				return;
			}

			ServantVO hired;
			if (model.Hired.TryGetValue(servant.slot, out hired) && hired != servant)
			{
				Debug.Log("Can't Hire that Servant, there's someone in that slot already");
				return;
			}

			servant.Hired = true;
			model.Introduced[servant.slot].Remove(servant);
			model.Hired.Add(servant.slot, servant);
			AmbitionApp.GetModel<GameModel>().Livre -= servant.Wage;
			Debug.Log(servant.NameAndTitle + " Hired!");
		}
	}
}