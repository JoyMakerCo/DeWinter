using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class AfterPartyUIIntroAnimator : MonoBehaviour
    {
        private const string ACTIVE = "Active"; //corresponds with Animation Parameter

        private Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            HandleIntroAnimation();
            AmbitionApp.Subscribe(PartyMessages.END_PARTY, HandleEndParty);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(PartyMessages.END_PARTY, HandleEndParty);
        }

        private void HandleIntroAnimation()
        {
            _animator.SetBool(ACTIVE, true);
        }

        private void HandleEndParty()
        {
            _animator.SetBool(ACTIVE, false);
        }
    }
}
