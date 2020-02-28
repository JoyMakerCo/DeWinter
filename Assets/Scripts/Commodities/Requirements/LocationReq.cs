namespace Ambition
{
    public static class LocationReq
    {
        // Checks the Paris model for known locations or currently available explorable locations
        public static bool Check(RequirementVO req)
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            return paris.Locations.Contains(req.ID) || (paris.Dailies != null && System.Array.IndexOf(paris.Dailies, req.ID) >= 0);
        }
    }
}
