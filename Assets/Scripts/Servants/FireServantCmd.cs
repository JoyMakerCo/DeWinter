using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
    public class FireServantCmd : ICommand<string>
    {
        public void Execute(string servantID)
        {
            ServantModel model = AmbitionApp.GetModel<ServantModel>();
            ServantVO servant = model.GetServant(servantID);
            if (servant != null && servant.Status == ServantStatus.Hired)
            {
                servant.Status = ServantStatus.Introduced;
                model.Broadcast();
            }
        }
    }
}
