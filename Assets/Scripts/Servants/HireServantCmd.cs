using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace DeWinter
{
	public class HireServantCmd : ICommand<string>
	{
		public void Execute (string servantName)
		{
			ServantModel model = DeWinterApp.GetModel<ServantModel>();
			ServantVO servant = Array.Find(model.Servants, s => s.Name == servantName);
			if (servant == null || !servant.Introduced)
			{
				Debug.Log("Can't Hire a Servant who hasn't been introduced yet");
				return;
			}

			if (model.Hired.ContainsKey(servant.slot))
			{
				Debug.Log("Can't Hire that Servant, there's someone in that slot already");
				return;
			}

			servant.Hired = true;
			model.Introduced.Remove(servant);
			model.Hired.Add(servant.slot, servant);
			AdjustBalanceVO msg = new AdjustBalanceVO("Livre", -servant.Wage);
			DeWinterApp.SendMessage<AdjustBalanceVO>(msg);
			Debug.Log(servant.NameAndTitle + " Hired!");
		}
	}
}