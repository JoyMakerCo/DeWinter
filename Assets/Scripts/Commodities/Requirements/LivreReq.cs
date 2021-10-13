using System;
namespace Ambition
{
    public static class LivreReq
    {
        public static bool Check(RequirementVO req)
        {
            int value = AmbitionApp.GetModel<GameModel>().Livre;
            return RequirementsSvc.Check(req, value);
        }
    }
}
