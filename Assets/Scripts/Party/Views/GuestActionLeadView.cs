using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class GuestActionLeadView : GuestActionIcon
    {
        private const float METER_TIME = 1f;

        public Image Meter;
        public Image Reward;
        public SpriteConfig RewardIcons;

        private GuestActionVO _action;

        void OnEnable()
        {
            AmbitionApp.Subscribe<int>(PartyMessages.ROUND, HandleRound);
        }

        void OnDisable()
        {
            AmbitionApp.Unsubscribe<int>(PartyMessages.ROUND, HandleRound);
            StopAllCoroutines();
            _action = null;
        }

        public override void SetAction(GuestActionVO action)
        {
            _action = action;
            gameObject.SetActive(_action != null && _action.Type == "Lead");
            if (gameObject.activeSelf)
            {
                Reward.sprite = RewardIcons.GetSprite(action.Tags[0]);
                Meter.fillAmount = 0f;
            }
        }

        private void HandleRound(int round)
        {
            StopAllCoroutines();
            if (isActiveAndEnabled && _action != null && round < _action.Rounds)
            {
                StartCoroutine(NextRound(round));
            }
        }

        IEnumerator NextRound(int round)
        {
            float fill = Meter.fillAmount;
            float target = _action.Rounds > 1 ? (float)(round - _action.StartRound)/(float)(_action.Rounds - 1) : 1f;
            for (float t = 0; t < METER_TIME; t+=Time.deltaTime)
            {
                Meter.fillAmount = ((METER_TIME - t) * fill + t * target)/METER_TIME;
                yield return null;
            }
            Meter.fillAmount = target;
        }
    }
}
