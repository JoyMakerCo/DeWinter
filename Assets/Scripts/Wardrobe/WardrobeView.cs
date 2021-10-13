using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
    public class WardrobeView : SortableList<OutfitVO>
    {
        // PUBLIC DATA //////////////////////////////
        public Text OutfitNameText;
        public Text OutfitDescriptionText;
        public Text ItemCountText;
        public bool IsMerchant;
        public Selectable FactionEffectsBtn;
        public FactionTextEffect[] FactionEffects;
        public Color PositiveEffectTxtColor;
        public Color NeutralEffectTxtColor;
        public Color NegativeEffectTxtColor;

        // PUBLIC METHODS //////////////////////////////

        public override void Initialize()
        {
            if (IsMerchant) AmbitionApp.SendMessage(InventoryMessages.UPDATE_MERCHANT);
            AmbitionApp.Inventory.Observe(UpdateInventory);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.BROWSE, HandleOutfit);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
            if (FactionEffectsBtn != null) FactionEffectsBtn.interactable = false;
            HandleOutfit(AmbitionApp.Inventory.GetEquippedItem(ItemType.Outfit));
        }

        public override void Dispose()
        {
            AmbitionApp.Inventory.Unobserve(UpdateInventory);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.BROWSE, HandleOutfit);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
        }

        public void SetMerchantMode(bool isMerchant) => IsMerchant = isMerchant;

        public void UpdateCount() => ItemCountText.text = Count.ToString();

        // PRIVATE METHODS //////////////////////////////
        protected override OutfitVO[] FetchListData()
        {
            InventoryModel inventory = AmbitionApp.Inventory;
            if (IsMerchant) AmbitionApp.SendMessage(InventoryMessages.UPDATE_MERCHANT);
            List<ItemVO> items = IsMerchant
                ? inventory.Market.FindAll(i => i.Type == ItemType.Outfit)
                : new List<ItemVO>(inventory.GetItems(ItemType.Outfit));
            List<OutfitVO> outfits = new List<OutfitVO>();
            foreach (ItemVO item in items)
            {
                if (item is OutfitVO)
                    outfits.Add((OutfitVO)item);
            }
            return outfits.ToArray();
        }

        private void UpdateInventory(InventoryModel inventory)
        {
            Refresh();
            ItemCountText.text = Count.ToString();
        }

        private void HandleOutfit(ItemVO item)
        {
            OutfitVO outfit = item as OutfitVO;
            if (outfit != null)
            {
                InventoryModel inventory = AmbitionApp.Inventory;
                int credShift;
                if (FactionEffectsBtn != null)
                    FactionEffectsBtn.interactable = true;
                if (OutfitDescriptionText != null)
                    OutfitDescriptionText.text = GetOutfitDescription(outfit);
                OutfitNameText.text = AmbitionApp.Localization.GetItemName(outfit);
                foreach (FactionTextEffect effect in FactionEffects)
                {
                    credShift = inventory.GetFactionBonus(outfit, effect.Faction);
                    effect.Text.text = (credShift > 0 ? "+" : "") + credShift.ToString();
                    effect.Text.color = credShift > 0
                        ? PositiveEffectTxtColor
                        : credShift < 0
                        ? NegativeEffectTxtColor
                        : NeutralEffectTxtColor;
                    effect.Text.fontStyle = (credShift == 0) ? FontStyle.Normal : FontStyle.Bold;
                }
            }
            else
            {
                foreach (FactionTextEffect effect in FactionEffects)
                {
                    effect.Text.text = "0";
                    effect.Text.color = NeutralEffectTxtColor;
                    effect.Text.fontStyle = FontStyle.Normal;
                }
            }
        }

        protected override Comparison<OutfitVO> GetComparer(int sortIndex)
        {
            InventoryModel inventory = AmbitionApp.Inventory;
            // Careful here, this is using the loc key to select the comparer. It's definitely cheating.
            if (Enum.TryParse<FactionType>(SortParams[sortIndex], true, out FactionType faction) && faction != FactionType.None)
            {
                return (x, y) => -inventory.GetFactionBonus(x,faction).CompareTo(inventory.GetFactionBonus(y, faction));
            }
            return (x, y) => -x.Price.CompareTo(y.Price);
        }

        protected string GetOutfitDescription(OutfitVO outfit)
        {
            if (outfit == null) return default;
            string result = AmbitionApp.Localize(outfit.ID + ItemConsts.ITEM_LOC_DESCRIPTION);
            if (!string.IsNullOrWhiteSpace(result)) return result;

            List<string> phrases = new List<string>(AmbitionApp.GetPhrases("outfit.luxury").Values);
            Dictionary<string, string> subs = new Dictionary<string, string>();
            subs["%l"] = phrases[(int)Math.Floor(phrases.Count * (100 + outfit.Luxury) * .005)]; ;
            phrases = new List<string>(AmbitionApp.GetPhrases("outfit.modesty").Values);
            subs["%m"] = phrases[(int)Math.Floor(phrases.Count * (100 + outfit.Modesty) * .005)];
            return AmbitionApp.Localize("outfit.description", subs);
        }

        [Serializable]
        public struct FactionTextEffect
        {
            public FactionType Faction;
            public Text Text;
        }
    }
}
