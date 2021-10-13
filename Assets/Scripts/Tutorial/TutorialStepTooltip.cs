using System;
using System.Collections.Generic;
using UnityEngine;
namespace Ambition
{
    public class TutorialStepTooltip : MonoBehaviour
    {
        public string StepID;
        public GameObject TooltipObject;

        private void Awake()
        {
            AmbitionApp.Subscribe<string>(TutorialMessages.TUTORIAL_STEP, HandleStep);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(TutorialMessages.TUTORIAL_STEP, HandleStep);
        }

        private void HandleStep(string stepID)
        {
            TooltipObject.SetActive(stepID == StepID);
        }
    }
}
