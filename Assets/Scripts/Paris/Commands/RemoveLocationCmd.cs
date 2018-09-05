using System;
using Core;

namespace Ambition
{
    public class AddLocationCmd : ICommand<string>
    {
        public void Execute(string location)
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            if (!paris.Locations.Contains(location))
                paris.Locations.Add(location);
        }
    }
}
