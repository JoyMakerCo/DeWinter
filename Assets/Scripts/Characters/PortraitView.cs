using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class PortraitView : MonoBehaviour
    {
        [HideInInspector]
        public string Tooltip;
        public Image Portrait;

        public CharacterVO Character
        {
            set
            {
                Avatar = value?.Avatar ?? default;
                Tooltip = value.Name;
            }
        }

        public AvatarVO Avatar
        {
            set
            {
                gameObject.SetActive(value.Portrait != null);
                Portrait.sprite = value.Portrait;
            }
        }
    }
}
