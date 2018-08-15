using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class ParisModel : IModel
    {
        public Dictionary<string, LocationVO> Locations = new Dictionary<string, LocationVO>();
        public Dictionary<string, LocationVO> VisitedLocations = new Dictionary<string, LocationVO>();
    }
}
