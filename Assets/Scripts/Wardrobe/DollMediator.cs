using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class DollMediator : MonoBehaviour
    {
        public Image Doll;

        private Sprite _default;

        void Awake()
        {
            _default = Doll.sprite;
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.BROWSE, HandleOutfit);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.BROWSE, HandleOutfit);
        }

        void HandleOutfit(ItemVO outfit)
        {
            Doll.sprite = AmbitionApp.Inventory.GetAsset(outfit);
        }
    }
}
