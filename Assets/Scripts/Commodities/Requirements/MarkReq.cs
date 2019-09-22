using System;
using System.Globalization;

namespace Ambition
{
    public static class MarkReq
    {
        // Returns whether the Mark's room has been cleared
        public static bool Check(RequirementVO req)
        {
            MapModel map = AmbitionApp.GetModel<MapModel>();
            return true; //Array.Exists(map.Map.Rooms, r => r.HostHere && r.Cleared);
        }
    }
}
