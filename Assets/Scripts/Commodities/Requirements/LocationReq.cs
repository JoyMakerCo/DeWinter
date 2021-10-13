namespace Ambition
{
    public static class LocationReq
    {
        // Checks the Paris model for known locations or currently available explorable locations
        public static bool Check(RequirementVO req)
        {
            ParisModel paris = AmbitionApp.Paris;
            return (paris.Exploration.Contains(req.ID) || paris.Rendezvous.Contains(req.ID)) == (req.Value > 0);
        }
    }
}
