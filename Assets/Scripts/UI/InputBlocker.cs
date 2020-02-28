using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class InputBlocker : MonoBehaviour
    {
        public CanvasGroup Blocker;

        private void OnEnable()
        {
            AmbitionApp.Subscribe(GameMessages.LOCK_UI, Lock);
            AmbitionApp.Subscribe(GameMessages.UNLOCK_UI, Unlock);
        }

        private void OnDisable()
        {
            AmbitionApp.Unsubscribe(GameMessages.LOCK_UI, Lock);
            AmbitionApp.Unsubscribe(GameMessages.UNLOCK_UI, Unlock);
        }

        private void Lock() => Blocker.blocksRaycasts = true;
        private void Unlock() => Blocker.blocksRaycasts = false;
    }
}
