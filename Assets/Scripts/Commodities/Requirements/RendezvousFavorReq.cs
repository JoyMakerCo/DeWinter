using System;
namespace Ambition
{
    public static class RendezvousFavorReq
    {
        public static bool Check(RequirementVO req)
        {
            RendezVO rendez = AmbitionApp.GetEvent() as RendezVO;
            if (rendez == null) return false;

            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            CharacterVO character = model.GetCharacter(rendez.Character);
            return RequirementsSvc.Check(req, character?.Favor ?? 0);
        }
    }
}
