<<<<<<< Updated upstream:Assets/Scripts/Wardrobe/DollMediator.cs
﻿using System.Collections;
using System.Collections.Generic;
=======
﻿using System;
using System.Collections;
>>>>>>> Stashed changes:Assets/Scripts/Wardrobe/WardrobeView.cs
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
<<<<<<< Updated upstream:Assets/Scripts/Wardrobe/DollMediator.cs
	public class DollMediator : MonoBehaviour
	{
		public SpriteConfig DressConfig;

		private Image _dollImage;

		void Awake()
		{
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			_dollImage = GetComponent<Image>();
            HandleOutfit(inventory.GetEquippedItem(ItemType.Outfit));
		}

		void OnEnable()
		{
			AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
		}

		void OnDisable()
		{
			AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
		}

		private void HandleOutfit(ItemVO item)
		{
            if (item.Type == ItemType.Outfit)
=======
    public class WardrobeView : MonoBehaviour
    {
        public Button ExitButton;
        public OutfitConfig OutfitConfig;
        public Image Doll;

        private void Awake()
        {
            ExitButton.onClick.AddListener(() => AmbitionApp.SendMessage(GameMessages.EXIT_SCENE));
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.DISPLAY_ITEM, HandleOutfit);
        }

        private void HandleOutfit(ItemVO outfit)
        {
            if (outfit.Asset == null)
>>>>>>> Stashed changes:Assets/Scripts/Wardrobe/WardrobeView.cs
            {
                string style = OutfitWrapperVO.GetStyle(item);
                _dollImage.sprite = DressConfig.GetSprite(style) ?? DressConfig.Sprites[0].Sprite;
            }
        }
<<<<<<< Updated upstream:Assets/Scripts/Wardrobe/DollMediator.cs
	}
=======

        private void OnDestroy()
        {
            ExitButton.onClick.RemoveAllListeners();
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.DISPLAY_ITEM, HandleOutfit);
        }
    }
>>>>>>> Stashed changes:Assets/Scripts/Wardrobe/WardrobeView.cs
}
