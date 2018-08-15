using System;
using UnityEngine;

namespace Ambition
{
    public class LocationVO
    {
        public string Name;
        public int ID;
        public string IncidentID;
        public string Scene;
        public bool OneShot;
        public bool Discoverable;
        // Requirements(Configurable Requirements list)

        public LocationVO() {}
        public LocationVO(LocationPin pin)
        {
            Name = pin.name;
            ID = pin.GetInstanceID();
            Scene = pin.Scene?.name;
            OneShot = pin.OneShot;
            Discoverable = pin.Discoverable;
        }
    }
}
