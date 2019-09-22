using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
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
            {
                string style = OutfitWrapperVO.GetStyle(item);
                _dollImage.sprite = DressConfig.GetSprite(style) ?? DressConfig.Sprites[0].Sprite;
            }
        }
	}
}
