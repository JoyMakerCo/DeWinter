using System;
using Core;
using UnityEngine;

namespace DeWinter
{
	public class HireServantCmd : ICommand<string>
	{
		public void Execute (string servantName)
		{
			ServantModel model = DeWinterApp.GetModel<ServantModel>();
			ServantVO servant = model.GetServant(servantName);
			if (servant == null || !servant.introduced)
			{
				Debug.Log("Can't Hire a Servant who hasn't been introduced yet");
				return;
			}

			if (model.Servants.ContainsKey(servant.slot))
			{
				Debug.Log("Can't Hire that Servant, there's someone in that slot already");
			}

			servant.hired = true;
			model.Servants.Add(servant.slot, servant);
			GameData.moneyCount -= servant.wage;
			Debug.Log(servant.NameAndTitle + " Hired!");
		}
	}
}