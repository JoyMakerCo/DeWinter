using System;
using System.Globalization;

namespace Ambition
{
    public static class CharacterReq
    {
        public static bool Check(RequirementVO req)
        {
            CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(req.ID);
            int check = (character?.Acquainted ?? false) ? 1 : 0;
            return RequirementsSvc.Check(req, check);
        }
    }
}
