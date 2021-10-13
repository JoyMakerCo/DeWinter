using System;
using UnityEngine;
using UnityEngine.UI;
using Ambition;

namespace Ambition
{
    public class OutfitButtonMediator : SortableItem<OutfitVO>
    {
        public Text ItemName;
        public Image GiftIcon;
        public GameObject GiftIndicator;
        public SpriteConfig FactionSprites;

        private OutfitVO _outfit;
        public override OutfitVO Data
        {
            get => _outfit;
            set
            {
                _outfit = value;
                if (ItemName != null)
                {
                    ItemName.text = AmbitionApp.Localization.GetItemName(_outfit);
                }
            }
        }

        public void Browse()
        {
            if (_outfit != null)
                AmbitionApp.SendMessage(InventoryMessages.BROWSE, _outfit as ItemVO);
        }

        public void SetFaction(FactionType faction)
        {
            GiftIcon.sprite = FactionSprites.GetSprite(faction.ToString());
            GiftIndicator.SetActive(GiftIcon.sprite != null);
        }
    }
}
