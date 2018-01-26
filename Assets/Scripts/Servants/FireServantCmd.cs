using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class FireServantCmd : ICommand<ServantVO>
	{
		public void Execute (ServantVO servant)
		{
			ServantModel model = AmbitionApp.GetModel<ServantModel>();
			model.Fire(servant);
		}
	}
}
