using UnityEngine;

namespace Ambition
{
    public class TutorialHideView : MonoBehaviour
    {
        public string TutorialState;
        void Awake()
        {
            if (!AmbitionApp.IsActiveState(TutorialConsts.TUTORIAL))
                Destroy(this);
            this.gameObject.SetActive(false);
            AmbitionApp.Subscribe<string>(TutorialMessage.TUTORIAL_STEP, HandleStep);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(TutorialMessage.TUTORIAL_STEP, HandleStep);       
        }

        private void HandleStep(string step)
        {
            if (step == TutorialState)
            {
                this.gameObject.SetActive(true);
                Destroy(this);
            }
        }
    }
}
