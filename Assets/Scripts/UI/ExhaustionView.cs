using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
    public class ExhaustionView : MonoBehaviour
    {
        private const string EXHAUSTION_TOOLTIP = "exhaustion_tooltip";
        private const string WELL_RESTED_TOOLTIP = "well_rested_tooltip";
        private const string EXHAUSTION_LEVEL_TOKEN = "%l";
        private const string EXHAUSTION_VALUE_TOKEN = "%n";

        public Sprite[] ExhaustionIcons;
        public Sprite RestedIcon;
        public Image Indicator;
        public Text Tooltip;

        private void OnEnable()
        {
            AmbitionApp.Subscribe(ParisMessages.REST, UpdateView);
            UpdateView();
        }

        private void OnDisable() => AmbitionApp.Unsubscribe(ParisMessages.REST, UpdateView);

        private void UpdateView()
        {
            int exhaustion = AmbitionApp.Game.Exhaustion;
            int penalty = AmbitionApp.Game.ExhaustionPenalty;
            Dictionary<string, string> subs = new Dictionary<string, string>();
            subs[EXHAUSTION_VALUE_TOKEN] = penalty > 0 ? ("+" + penalty) : penalty.ToString();
            Indicator.enabled = exhaustion != 0;
            if (exhaustion > 0)
            {
                subs[EXHAUSTION_LEVEL_TOKEN] = exhaustion.ToString();
                Indicator.sprite = exhaustion <= ExhaustionIcons.Length
                    ? ExhaustionIcons[exhaustion - 1]
                    : ExhaustionIcons[ExhaustionIcons.Length - 1];
                Tooltip.text = AmbitionApp.Localize(EXHAUSTION_TOOLTIP, subs);
            }
            else
            {
                Indicator.sprite = RestedIcon;
                Tooltip.text = AmbitionApp.Localize(WELL_RESTED_TOOLTIP, subs);
            }
        }
    }
}
