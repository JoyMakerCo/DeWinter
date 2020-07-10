using System;
using System.Collections;
using UnityEngine;
using FMODUnity;
using Core;

namespace Ambition
{
    public class AudioLoader : MonoBehaviour
    {
        public StudioBankLoader Loader;
        public void Load()
        {
            AudioSvc svc = AmbitionApp.GetService<AudioSvc>();
            if (svc != null || Loader == null)
            {
                AmbitionApp.SendMessage(AudioMessages.ALL_SOUNDS_LOADED);
            }
            else
            {
                svc = App.Register<AudioSvc>();
                Loader.Load();
                svc.Load(Loader.Banks);
                StartCoroutine(WaitForBanks());
            }
        }

        IEnumerator WaitForBanks()
        {
            while (RuntimeManager.AnyBankLoading())
                yield return null;
            Destroy(Loader.gameObject);
            AmbitionApp.SendMessage(AudioMessages.ALL_SOUNDS_LOADED);
        }
    }
}
