﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class InputBlocker : MonoBehaviour
    {
        public CanvasGroup Blocker;

        private bool _interactible=true;

        private void OnEnable()
        {
            AmbitionApp.Subscribe(GameMessages.LOCK_UI, Lock);
            AmbitionApp.Subscribe(GameMessages.UNLOCK_UI, Unlock);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT, FadeLock);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, FadeLock);
            AmbitionApp.Subscribe(GameMessages.FADE_IN, FadeLock);
            AmbitionApp.Subscribe(GameMessages.FADE_IN_COMPLETE, FadeUnlock);
        }

        private void OnDisable()
        {
            AmbitionApp.Unsubscribe(GameMessages.LOCK_UI, Lock);
            AmbitionApp.Unsubscribe(GameMessages.UNLOCK_UI, Unlock);
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT, FadeLock);
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, FadeLock);
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN, FadeLock);
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN_COMPLETE, FadeUnlock);
        }

        private void Lock() => Blocker.blocksRaycasts = (_interactible = true);
        private void Unlock() => Blocker.blocksRaycasts = (_interactible = false);
        private void FadeLock() => Blocker.blocksRaycasts = false;
        private void FadeUnlock() => Blocker.blocksRaycasts = _interactible;
    }
}
