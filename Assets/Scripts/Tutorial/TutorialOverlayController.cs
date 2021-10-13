using System;
using UnityEngine;
using UnityEngine.UI;
using UFlow;
namespace Ambition
{
    public class TutorialOverlayController : MonoBehaviour
    {
        public TutorialStepMap[] Steps;

        private void OnEnable()
        {
            AmbitionApp.Subscribe<string>(TutorialMessages.TUTORIAL_STEP, HandleStep);
        }

        private void OnDisable()
        {
            AmbitionApp.Unsubscribe<string>(TutorialMessages.TUTORIAL_STEP, HandleStep);
        }

        private void HandleStep(string step)
        {
            foreach (TutorialStepMap map in Steps)
            {
                map.Popup?.gameObject.SetActive(map.StepID == step);
            }
        }

        private void HandleButton()
        {
            AmbitionApp.SendMessage(TutorialMessages.TUTORIAL_NEXT_STEP);
        }

        [Serializable]
        public struct TutorialStepMap
        {
            public string StepID;
            public GameObject Popup;
        }
    }
}
