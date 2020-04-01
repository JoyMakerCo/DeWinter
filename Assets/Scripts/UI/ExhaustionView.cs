using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
    public class ExhaustionView : MonoBehaviour
    {
        public Sprite[] ExhaustionIcons;
        public Sprite RestedIcon;
        public Image Indicator;
        public Text Tooltip;

        private GameModel _Model => AmbitionApp.GetModel<GameModel>();

        private void OnEnable() => _Model?.Exhaustion.Observe(HandleExhaustion);
        private void OnDisable() => _Model?.Exhaustion.Remove(HandleExhaustion);

        private void HandleExhaustion(int exhaustion)
        {
            Indicator.gameObject.GetComponent<CanvasRenderer>().SetAlpha(exhaustion != 0 ? 1 : 0);
            Indicator.raycastTarget = (exhaustion != 0);
            if (exhaustion > 0)
            {
                if (exhaustion > ExhaustionIcons.Length) exhaustion = ExhaustionIcons.Length;
                Indicator.sprite = ExhaustionIcons[exhaustion - 1];
                Tooltip.text = AmbitionApp.Localize("exhaustion_level_" + exhaustion.ToString());
            }
            else if (exhaustion < 0)
            {
                Indicator.sprite = RestedIcon;
                Tooltip.text = AmbitionApp.Localize("well_rested");
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(Tooltip?.GetComponentInParent<ContentSizeFitter>()?.GetComponent<RectTransform>());
        }

        private void HandleWellRested() => HandleExhaustion(-1);
    }
}
