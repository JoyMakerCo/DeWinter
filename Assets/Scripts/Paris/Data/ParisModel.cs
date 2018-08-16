using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class ParisModel : IModel, Util.IInitializable
    {
        public Dictionary<string, LocationVO> Locations;
        public Dictionary<string, LocationVO> VisitedLocations;

        public void Initialize()
        {
            Locations = new Dictionary<string, LocationVO>();
            VisitedLocations = new Dictionary<string, LocationVO>();
        }
    }
}
