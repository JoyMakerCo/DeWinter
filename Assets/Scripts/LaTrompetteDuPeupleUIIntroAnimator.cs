﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class LaTrompetteDuPeupleUIIntroAnimator : MonoBehaviour
    {
        private const string ACTIVE = "Active"; //corresponds with Animation Parameter

        private Animator _animator;

        void Awake()
        {
            string title = AmbitionApp.Localize("parismap.locationlabel.la_trompette_du_peuple");
            _animator = GetComponent<Animator>();
            HandleIntroAnimation();
            AmbitionApp.Subscribe(CalendarMessages.NEXT_DAY, HandleLeaveLocation);
            AmbitionApp.SendMessage(GameMessages.SHOW_HEADER, title);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(CalendarMessages.NEXT_DAY, HandleLeaveLocation);
        }

        private void HandleIntroAnimation()
        {
            _animator.SetBool(ACTIVE, true);
        }

        private void HandleLeaveLocation()
        {
            _animator.SetBool(ACTIVE, false);
        }
    }
}
