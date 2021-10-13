using System;
namespace Ambition
{
    public static class RendezvousOutfitReq
    {
        public static bool Check(RequirementVO req)
        {
            RendezVO rendez = AmbitionApp.GetEvent() as RendezVO;
            if (rendez == null) return false;

            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            int score = model.GetOutfitFavor(rendez.Character, AmbitionApp.Inventory.GetEquippedItem(ItemType.Outfit) as OutfitVO);
            return RequirementsSvc.Check(req, score);
        }
    }
}
