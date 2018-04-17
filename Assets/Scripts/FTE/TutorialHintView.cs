using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ambition
{
    // Be sure the name of this GameObject matches the name of the Tutorial Step. 
    public class TutorialHintView : MonoBehaviour, IPointerClickHandler
    {
        void Awake()
        {
            bool isState = AmbitionApp.IsActiveState(this.gameObject.name);
            this.gameObject.SetActive(isState);
            if (!isState)
            {
                AmbitionApp.Subscribe<string>(TutorialMessage.TUTORIAL_STEP, HandleTutorialStep);
            }
            AmbitionApp.Subscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleTutorialStepComplete);
        }
        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(TutorialMessage.TUTORIAL_STEP, HandleTutorialStep);
            AmbitionApp.Unsubscribe<string>(TutorialMessage.TUTORIAL_STEP_COMPLETE, HandleTutorialStepComplete);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Destroy(this.gameObject);
        }

        private void HandleTutorialStep(string step)
        {
            if (step == this.gameObject.name)
                this.gameObject.SetActive(true);
        }

        private void HandleTutorialStepComplete(string step)
        {
            if (step == this.gameObject.name)
                this.gameObject.SetActive(false);
        }
    }
}
