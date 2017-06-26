using System;
using Core;
using UnityEngine;

namespace Ambition
{
	public class FireServantCmd : ICommand<ServantVO>
	{
		public void Execute (ServantVO servant)
		{
			ServantModel model = DeWinterApp.GetModel<ServantModel>();

			if (model.Hired.ContainsKey(servant.slot) && model.Hired[servant.slot] == servant)
			{
				servant.Hired = false;
				servant.Introduced = false;
				model.Hired.Remove(servant.slot);
				Debug.Log(servant.NameAndTitle + " Fired!");
			}
		}
	}
}