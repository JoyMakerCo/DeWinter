using System;
namespace Ambition
{
    public class RendezvousOutfitReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO commodity)
        {
            RendezVO rendez = AmbitionApp.GetEvent() as RendezVO;
            if (rendez == null) return;

            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            CommodityVO reward = new CommodityVO()
            {
                Type = CommodityType.Favor,
                ID = rendez.Character,
                Value = model.GetOutfitFavor(rendez.Character, AmbitionApp.Inventory.GetEquippedItem(ItemType.Outfit) as OutfitVO)
            };
            AmbitionApp.SendMessage(reward);
        }
    }
}
