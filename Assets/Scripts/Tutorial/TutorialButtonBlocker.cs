using UnityEngine;
using System; 
using System.Collections;

namespace Ambition
{
    public class TutorialButtonBlocker : MonoBehaviour
    {
        public string[] UnblockSteps;

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
            this.gameObject.SetActive(Array.IndexOf(UnblockSteps, stepID) < 0);
        }
    }
}
