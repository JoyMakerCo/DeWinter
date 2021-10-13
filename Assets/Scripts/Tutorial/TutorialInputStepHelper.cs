using System;
using UnityEngine;
using UFlow;
namespace Ambition
{
    public class TutorialInputStepHelper : MonoBehaviour
    {
        public string FlowID;
        public string InputStateID;
        public string TargetStateID;

        private void Start()
        {
            UMachine[] flows = AmbitionApp.UFlow.GetFlows(FlowID);
            bool setActive = Array.Exists(flows, f => Array.IndexOf(f.GetActiveStates(), InputStateID) >= 0);
            if (setActive) AmbitionApp.Subscribe<string>(TutorialMessages.TUTORIAL_STEP, OnNextStep);
            gameObject.SetActive(setActive);
        }

        private void OnDisable() => AmbitionApp.Unsubscribe<string>(TutorialMessages.TUTORIAL_STEP, OnNextStep);
        private void OnNextStep(string stateID)
        {
            if (stateID == TargetStateID)
                gameObject.SetActive(false);
        }
    }
}
