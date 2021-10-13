using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ambition
{
    public class AnimationEventListener : MonoBehaviour
    {
        public Animator Animator;
        public AnimationEventTrigger[] Triggers;

        public void TriggerAnimationEvent(string eventID)
        {
            AnimationEventTrigger trigger = Array.Find(Triggers, t => t.EventID == eventID);
            trigger.Events?.Invoke();
        }

        [Serializable]
        public struct AnimationEventTrigger
        {
            public string EventID;
            public UnityEvent Events;
        }
    }
}
