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
        public CommodityVO[] Requirements;
    }
}
