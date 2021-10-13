using System;
using UnityEngine;
using UFlow;
namespace Ambition
{
    public class TutorialOverlay : MonoBehaviour
    {
        public string MachineID;
        public GameObject TutorialPrefab;

        private GameObject _instance = null;

        private void OnEnable()
        {
            if (AmbitionApp.Game.Tutorials.Contains(MachineID))
            {
                UFlowSvc uFlow = AmbitionApp.GetService<UFlowSvc>();
                if (uFlow.IsActiveFlow(MachineID) || uFlow.FlowExists(MachineID))
                {
                    if (_instance == null)
                    {
                        _instance = Instantiate<GameObject>(TutorialPrefab, this.transform);
                    }
                    AmbitionApp.Subscribe<string>(TutorialMessages.END_TUTORIAL, HandleEndTutorial);
                    AmbitionApp.SendMessage(TutorialMessages.START_TUTORIAL, MachineID);
                }

            }
            else GameObject.Destroy(this.gameObject);
        }

        private void OnDisable()
        {
            AmbitionApp.Unsubscribe<string>(TutorialMessages.END_TUTORIAL, HandleEndTutorial);
        }

        private void HandleEndTutorial(string machineID)
        {
            if (MachineID == machineID)
            {
                Destroy(this.gameObject);
                if (_instance != null) Destroy(_instance);
                _instance = null;
            }
        }
    }
}
