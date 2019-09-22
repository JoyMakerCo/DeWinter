using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class PerilChangeTextView : MonoBehaviour
    {
        public Animator Animator;
        public Text Text;

        private int _peril = 0;

        void OnEnable() => AmbitionApp.Subscribe<int>(GameConsts.PERIL, HandlePeril);
        void OnDisable() => AmbitionApp.Unsubscribe<int>(GameConsts.PERIL, HandlePeril);

        private void HandlePeril(int peril)
        {
            if (peril > _peril)
            {
                Text.text = AmbitionApp.GetString("rewards.peril_up");
                Animator.SetTrigger("Negative");
            }
            else if (peril < _peril)
            {
                Text.text = AmbitionApp.GetString("rewards.peril_down");
                Animator.SetTrigger("Charmed");
            }
             _peril = peril;
        }
    }
}

