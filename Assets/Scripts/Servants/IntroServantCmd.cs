using System;
using Core;

namespace Ambition
{
	public class IntroServantCmd : ICommand<string>
	{
		public void Execute (string servantID)
		{
			ServantModel model = AmbitionApp.GetModel<ServantModel>();
            ServantVO servant = model.GetServant(servantID);
            if (servant == null)
            {
                servant = model.GetServant(servantID);
                if (servant != null)
                {
                    servant.Status = ServantStatus.Introduced;
                    model.Broadcast();
                }
            }
        }
    }
}
