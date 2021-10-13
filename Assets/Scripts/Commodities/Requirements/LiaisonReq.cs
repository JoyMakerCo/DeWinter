using System;
namespace Ambition
{
    public static class LiaisonReq
    {
        public static bool Check(RequirementVO req)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            model.Characters.TryGetValue(req.ID, out CharacterVO character);
            return character?.IsDateable ?? false;
        }
    }
}
