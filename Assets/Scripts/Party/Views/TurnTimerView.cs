using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
    public class TurnTimerView : MonoBehaviour
    {
        public Sprite TurnImg;
        public Sprite NoTurnImg;

        public Image[] TurnsLeft;
        public Text TurnsTxt;
        public Text TotalTurnsTxt;

        private void OnEnable()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            TotalTurnsTxt.text = model.Turns.ToString();
            TurnsTxt.text = model.TurnsLeft.ToString();
            for (int i = TurnsLeft.Length - 1; i >= 0; i--)
            {
                TurnsLeft[i].sprite = model.TurnsLeft > i ? TurnImg : NoTurnImg;
            }
        }
    }
}
