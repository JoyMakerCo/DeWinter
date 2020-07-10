using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class DollMediator : MonoBehaviour
    {
        public OutfitConfig OutfitConfig;
        public Image Doll;

        void Awake()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.DISPLAY_ITEM, HandleOutfit);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.DISPLAY_ITEM, HandleOutfit);
        }

        void HandleOutfit(ItemVO outfit)
        {
            Doll.sprite = OutfitConfig.GetOutfit(outfit);
        }
    }
}
