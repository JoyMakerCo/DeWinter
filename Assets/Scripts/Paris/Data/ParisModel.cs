using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class ParisModel : IModel, Util.IInitializable
    {
        public List<string> Locations;
        public List<string> Visited;

        public void Initialize()
        {
            Locations = new List<string>();
            Visited = new List<string>();
        }
    }
}
