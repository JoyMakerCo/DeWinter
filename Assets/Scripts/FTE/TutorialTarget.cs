using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dialog;

namespace Ambition
{
    public class TutorialTarget : MonoBehaviour, IPointerClickHandler
    {
        public string TutorialStep;
        void Awake()
        {
            if (!AmbitionApp.IsActiveMachine(TutorialConsts.TUTORIAL_MACHINE)) Destroy(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AmbitionApp.SendMessage<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, TutorialStep);
        }
    }
}
