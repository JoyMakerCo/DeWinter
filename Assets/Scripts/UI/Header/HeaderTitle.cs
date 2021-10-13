using System;
using UnityEngine;
using UnityEngine.UI;
using Util;
namespace Ambition
{
    public class HeaderTitle : MonoBehaviour
    {
        public Text Title;
        public Transform OverlayRoot;
        public PrefabMap[] Overlays;

        public void Awake()
        {
            LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
            AmbitionApp.Subscribe<string>(GameMessages.SHOW_HEADER, SetHeaderTitle);
            AmbitionApp.Subscribe<string>(GameMessages.SCENE_LOADED, HandleSceneLoaded);
            AmbitionApp.Subscribe(GameMessages.HIDE_HEADER, ClearOverlays);
            SetHeaderTitle(model.HeaderTitlePhrase);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(GameMessages.SHOW_HEADER, SetHeaderTitle);
            AmbitionApp.Unsubscribe<string>(GameMessages.SCENE_LOADED, HandleSceneLoaded);
            AmbitionApp.Unsubscribe(GameMessages.HIDE_HEADER, ClearOverlays);
        }

        private void SetHeaderTitle(string title)
        {
            AmbitionApp.GetModel<LocalizationModel>().HeaderTitlePhrase = title;
            string text = AmbitionApp.Localize(title);
            Title.text = string.IsNullOrEmpty(text) ? title : text;
        }

        private void HandleSceneLoaded(string sceneID)
        {
            foreach (PrefabMap map in Overlays)
            {
                ClearOverlays();
                if (map.ID == sceneID)
                {
                    Instantiate<GameObject>(map.Prefab, OverlayRoot);
                }
            }
        }

        private void ClearOverlays()
        {
            foreach (Transform child in OverlayRoot)
            {
                Destroy(child.gameObject);
            }
        }

    }
}
