using System;
using Core;

namespace Ambition
{
	public class IntroServantCmd : ICommand<ServantVO>
	{
		public void Execute (ServantVO servant)
		{
			ServantModel model = AmbitionApp.GetModel<ServantModel>();
			model.Introduce(servant);
		}
	}
}
