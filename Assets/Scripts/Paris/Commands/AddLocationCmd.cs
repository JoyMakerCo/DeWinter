using System;
using Core;

namespace Ambition
{
    public class RemoveLocationCmd : ICommand<string>
    {
        public void Execute(string location)
        {
            AmbitionApp.GetModel<ParisModel>().Locations.Remove(location);
        }
    }
}
