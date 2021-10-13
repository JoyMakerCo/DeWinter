using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class TextColorSwitcher : MonoBehaviour
    {
        public Text TextComponent;
        public Color EnabledColor;
        public Color DisabledColor;
        public void SwitchColor(bool enabled) => TextComponent.color = enabled
            ? EnabledColor
            : DisabledColor;
    }
}
