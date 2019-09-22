using System;
namespace Ambition
{
    public static class FavorReq
    {
        public static bool Check(RequirementVO req)
        {
            CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
            int favor = characters.Characters.TryGetValue(req.ID, out CharacterVO character)
                ? character.Favor : 0;
            return RequirementsSvc.Check(req, favor);
        }
    }
}
