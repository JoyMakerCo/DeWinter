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
    }
}
