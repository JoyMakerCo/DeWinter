using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class CredibilityChangeTextView : MonoBehaviour
    {
        public Animator Animator;
        public Text Text;

        private int _cred = 0;

        void OnEnable() => AmbitionApp.Subscribe<int>(GameConsts.CRED, HandleCredibility);
        void OnDisable() => AmbitionApp.Unsubscribe<int>(GameConsts.CRED, HandleCredibility);

        private void HandleCredibility(int credibility)
        {
             if (credibility > _cred)
            {
                Text.text = AmbitionApp.Localize("rewards.credibility_up");
                Animator.SetTrigger("Neutral");
            }
            else if (credibility < _cred)
            {
                Text.text = AmbitionApp.Localize("rewards.credibility_down");
                Animator.SetTrigger("Negative");
            }
            _cred = credibility;
        }
    }
}

