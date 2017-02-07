using System;
using Core;
using UnityEngine;

namespace DeWinter
{
	public class FireServantCmd : ICommand<ServantVO>
	{
		public void Execute (ServantVO servant)
		{
			ServantModel model = DeWinterApp.GetModel<ServantModel>();

			if (model.Servants.ContainsValue(servant))
			{
				servant.hired = false;
				servant.introduced = false;
				model.Servants.Remove(servant.slot);
				Debug.Log(servant.NameAndTitle + " Fired!");
			}
		}
	}
}