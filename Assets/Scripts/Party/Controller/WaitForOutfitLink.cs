using System;
using UFlow;

namespace Ambition
{
    public class WaitForOutfitLink : ULink
    {
        InventoryModel _inventory;
        public override void Initialize()
        {
            _inventory = AmbitionApp.GetModel<InventoryModel>();
            AmbitionApp.Subscribe(PartyMessages.START_PARTY, HandleCheckOutfit);
        }

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe(PartyMessages.START_PARTY, HandleCheckOutfit);
        }

        private void HandleCheckOutfit()
        {
            ItemVO item;
            if (_inventory.Equipped.TryGetValue(ItemConsts.OUTFIT, out item) && item is OutfitVO)
                Activate();
        }
    }
}
