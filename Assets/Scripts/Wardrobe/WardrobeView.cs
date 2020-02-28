﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
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
            {
                OutfitConfig.GetOutfit(outfit);
            }
            Doll.sprite = outfit.Asset;
        }

        private IEnumerator LoadAssetBundle(Action<AssetBundle> onComplete)
        {
            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(AmbitionApp.GetModel<GameModel>().PlayerPhrase.ToLower() + "_outfits");
            yield return req;
            onComplete(req?.assetBundle);
        }

        private void OnDestroy()
        {
            ExitButton.onClick.RemoveAllListeners();
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.DISPLAY_ITEM, HandleOutfit);
        }
    }
}
