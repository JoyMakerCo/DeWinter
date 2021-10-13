using System;
namespace Ambition
{
    public class FireServantTypeCmd : Core.ICommand<ServantType>
    {
        public void Execute(ServantType type)
        {
            ServantModel model = AmbitionApp.GetModel<ServantModel>();
            ServantVO servant = model.GetServant(type);
            if (servant != null)
            {
                servant.Status = ServantStatus.Introduced;
                model.Broadcast();
            }
        }
    }
}
