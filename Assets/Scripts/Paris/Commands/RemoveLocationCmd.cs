using System;
using Core;

namespace Ambition
{
    public class AddLocationCmd : ICommand<string>
    {
        public void Execute(string location)
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            if (!paris.Explorable.ContainsKey(location) && !paris.Known.Contains(location))
                paris.Known.Add(location);
        }
    }
}
