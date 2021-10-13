using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class JournalView : MonoBehaviour
    {
        public Button ActiveTab;
        private Sprite _defaultSprite;

        public void SetTab(Button tab)
        {
            if (ActiveTab != null)
            {
                ActiveTab.interactable = true;
                ActiveTab.image.sprite = _defaultSprite;
            }
            ActiveTab = tab;
            if (ActiveTab != null)
            {
                _defaultSprite = ActiveTab.image.sprite;
                ActiveTab.interactable = false;
                ActiveTab.image.sprite = ActiveTab.spriteState.pressedSprite;
            }
        }
        
        private void OnEnable()
        {
            _defaultSprite = ActiveTab.image.sprite;
            SetTab(ActiveTab);
        }

        private void OnDisable()
        {
            ActiveTab.image.sprite = _defaultSprite;
        }
    }
}
