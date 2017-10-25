using System;
using Core;

namespace Ambition
{
	public class IntroServantCmd : ICommand<ServantVO>
	{
		public void Execute (ServantVO servant)
		{
			ServantModel model = AmbitionApp.GetModel<ServantModel>();
			servant = Array.Find(model.Servants, s=>s.ID == servant.ID);
			if (servant != null)
			{
				servant.Introduced = true;
				if (!model.Introduced.Contains(servant)) model.Introduced.Add(servant);
			}
		}
	}
}
