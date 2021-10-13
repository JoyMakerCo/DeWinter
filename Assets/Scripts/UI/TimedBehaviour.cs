using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ambition
{
    public class TimedBehaviour : MonoBehaviour
    {
        public float Time;
        public TimedEvent Events;
        public bool StartOnEnable = true;

        public void StartTimer() => StartCoroutine(StartTimerCR(Time));
        public void StartTimer(float time) => StartCoroutine(StartTimerCR(time));

        private void OnEnable()
        {
            if (StartOnEnable) StartCoroutine(StartTimerCR(Time));
        }

        IEnumerator StartTimerCR(float time)
        {
            while(time > 0)
            {
                time -= UnityEngine.Time.deltaTime;
                yield return null;
            }
            Events.Invoke();
        }

        [System.Serializable]
        public class TimedEvent : UnityEvent { }
    }
}
