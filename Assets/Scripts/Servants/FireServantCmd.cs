using System;
using Core;
using UnityEngine;

namespace Ambition
{
	public class FireServantCmd : ICommand<ServantVO>
	{
		public void Execute (ServantVO servant)
		{
			ServantModel model = AmbitionApp.GetModel<ServantModel>();
			if (!servant.Tags.Contains(ServantConsts.PERMANENT))
			{
				model.Hired.Remove(servant.Slot);
				model.Introduced.Remove(servant);
				servant.Introduced = servant.Hired = false;
			}
			else
			{
			// Pop open a window
			}
		}
	}
}
