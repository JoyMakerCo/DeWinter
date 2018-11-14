using Core;

namespace Ambition
{
    public class AfterPartyCmd : ICommand
    {
        public void Execute()
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            // Update the "Last Equipped" outfit in the player's inventory

            OutfitVO outfit = inventory.GetEquipped(ItemConsts.OUTFIT) as OutfitVO;
            if (outfit != null) outfit.Novelty -= inventory.NoveltyDamage;

            inventory.Equipped[ItemConsts.LAST_OUTFIT] = outfit;
            // Reset the player's equipped items
            inventory.Equipped.Remove(ItemConsts.OUTFIT);
            inventory.Equipped.Remove(ItemConsts.ACCESSORY);
        }
    }
}
