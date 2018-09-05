using Util;
using Core;

namespace Ambition
{
    public class ExploreParisCmd : ICommand<LocationPin[]>
    {
        public void Execute(LocationPin[] locations)
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            int chance = (int)(model.ExplorelocationChance * 100);
            foreach(LocationPin pin in locations)
            {
                if (RNG.Generate(100) < chance)
                {
                    AmbitionApp.SendMessage(pin);
                    return;
                }
            }
            //AmbitionApp.SendMessage(ParisMessages.INCIDENT);
        }
    }
}
