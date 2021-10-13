using System;
using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public class TutorialButtonTarget : MonoBehaviour
    {
        public Button TutorialButton;
        public string TutorialID;
        public string StepID;
        private void Start()
        {
            if (!AmbitionApp.Game.Tutorials.Contains(TutorialID))
                Destroy(this);
            else if (string.IsNullOrEmpty(StepID))
                TutorialButton.onClick.AddListener(HandleClick);
            else  AmbitionApp.Subscribe<string>(TutorialMessages.TUTORIAL_STEP, HandleStep);
        }
        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(TutorialMessages.TUTORIAL_STEP, HandleStep);
            TutorialButton.onClick.RemoveListener(HandleClick);
        }
        private void HandleStep(string step)
        {
            if (step == StepID)
            {
                TutorialButton.onClick.AddListener(HandleClick);
            }
        }
        private void HandleClick()
        {
            AmbitionApp.SendMessage(TutorialMessages.TUTORIAL_NEXT_STEP);
            Destroy(this);
        }
    }
}
