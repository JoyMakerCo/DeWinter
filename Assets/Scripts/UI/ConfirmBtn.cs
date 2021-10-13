using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ambition
{
    public class ConfirmBtn : MonoBehaviour
    {
        public Text ButtonLabel;
        public Text ConfirmLabel;
        public OnConfirmEvent Event;

        public int WaitSeconds = 5;

        private bool _idle = true;

        public void OnClick()
        {
            if (_idle) StartCoroutine(WaitForConfirm());
            else
            {
                Event?.Invoke();
                StopAllCoroutines();
            }
            SetIdle(!_idle);
        }

        IEnumerator WaitForConfirm()
        {
            for (float t = (float)WaitSeconds; t>0; t-=Time.deltaTime)
            {
                yield return null;
            }
            SetIdle(true);
        }

        public void Cancel()
        {
            StopAllCoroutines();
            SetIdle(true);
        }

        private void SetIdle(bool value)
        {
            _idle = value;
            ButtonLabel.enabled = _idle;
            ConfirmLabel.enabled = !_idle;
        }
    }

    [System.Serializable]
    public class OnConfirmEvent : UnityEvent { }
}
