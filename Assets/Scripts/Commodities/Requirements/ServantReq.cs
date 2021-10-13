namespace Ambition
{
    public static class ServantReq
    {
        // Checks both the "equipped" servant titles and the names of the servants employed
        public static bool Check(RequirementVO req)
        {
            ServantVO servant = AmbitionApp.GetModel<ServantModel>().GetServant(req.ID);
            return RequirementsSvc.Check(req, (int)(servant?.Status ?? ServantStatus.Unknown));
        }
    }
}
