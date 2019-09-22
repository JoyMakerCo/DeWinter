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

        private PartyModel _model;

        void Awake()
        {
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, HandleMap);
            AmbitionApp.Subscribe(GameMessages.SHOW_HEADER, OnShow);
            AmbitionApp.Subscribe(GameMessages.HIDE_HEADER, OnShow);
            gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, HandleMap);
            AmbitionApp.Unsubscribe(GameMessages.SHOW_HEADER, OnShow);
            AmbitionApp.Unsubscribe(GameMessages.HIDE_HEADER, OnShow);
        }

        private void HandleMap()
        {
            if (_model == null) _model = AmbitionApp.GetModel<PartyModel>();
            gameObject.SetActive(true);
            TotalTurnsTxt.text = _model.Turns.ToString();
            TurnsTxt.text = _model.TurnsLeft.ToString();
            for (int i=TurnsLeft.Length-1; i>=0; i--)
            {
                TurnsLeft[i].sprite = _model.TurnsLeft > i ? TurnImg : NoTurnImg;
            }
        }

        private void OnShow() => gameObject.SetActive(false);
    }
}
