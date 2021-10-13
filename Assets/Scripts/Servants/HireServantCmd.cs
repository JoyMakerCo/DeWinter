using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class HireServantCmd : ICommand<string>
	{
		public void Execute (string servantID)
		{
			ServantModel model = AmbitionApp.GetModel<ServantModel>();
            ServantVO servant = model.LoadServant(servantID);
            if (servant != null && !servant.IsHired)
            {
                if (model.GetServant(servant.Type)?.ID != servant.ID)
                {
                    AmbitionApp.SendMessage(ServantMessages.FIRE_SERVANT, servant.Type);
                }
                servant.Status = ServantStatus.Hired;
                model.Broadcast();
            }
		}
	}
}
