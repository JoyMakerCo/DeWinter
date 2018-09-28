using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class OneShotSFXPlayer : MonoBehaviour
    {
        private static GameObject _instance = null;

        void Awake()
        {
            if (_instance != null) Destroy(this.gameObject);
            else
            {
                _instance = this.gameObject;
                DontDestroyOnLoad(_instance);
                AmbitionApp.Subscribe<FMODEvent>(AudioMessages.PLAY_ONESHOTSFX, HandlePlay);
            }
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<FMODEvent>(AudioMessages.PLAY_ONESHOTSFX, HandlePlay);
        }

        private void HandlePlay(FMODEvent ambientSFX)
        {
            FMODUnity.RuntimeManager.PlayOneShot(ambientSFX.Name);
        }
    }
}
