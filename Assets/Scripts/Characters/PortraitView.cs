using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class PortraitView : MonoBehaviour
    {
        public Image Portrait;

        [HideInInspector]
        public string Tooltip;

        [HideInInspector]
        public Tooltip TooltipObject;
    }
}
