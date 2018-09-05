using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
	public class ParisModel : IModel, Util.IInitializable
    {
<<<<<<< HEAD
        private LocationPin _location;
        public LocationPin Location
        {
            get { return _location; }
            set {
                _location = value;
                AmbitionApp.SendMessage(_location);
            }
        }

        public float ExplorelocationChance = .5f;

=======
>>>>>>> 9f7f794e52eac68e41e333d01759c8bbe33fa384
        public List<string> Locations;
        public List<string> Visited;

        public void Initialize()
        {
            Locations = new List<string>();
            Visited = new List<string>();
        }
    }
}
