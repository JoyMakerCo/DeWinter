using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    //I know it's super kludgy to have an ambient SFX player that is extremely similar in function to the Music player, I can smooth this out later
    //The central problem is that the AudioMessagers are heard by EVERYTHING, so using any kind of generalized play audio message effects both the Music Player and the ambient SFX player

    public class AmbientSFXPlayer : MonoBehaviour
    {
        private FMOD.Studio.EventInstance CurrentAmbientSFX;
        private static GameObject _instance = null;

        void Awake()
        {
            if (_instance != null) Destroy(this.gameObject);
            else
            {
                _instance = this.gameObject;
                DontDestroyOnLoad(_instance);
                AmbitionApp.Subscribe<FMODEvent>(AudioMessages.PLAY_AMBIENTSFX, HandlePlay);
                AmbitionApp.Subscribe(AudioMessages.STOP_AMBIENTSFX, HandleStop);
                AmbitionApp.Subscribe(AudioMessages.STOP_AMBIENTSFX_NOW, HandleStopNow);
            }
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<FMODEvent>(AudioMessages.PLAY_AMBIENTSFX, HandlePlay);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_AMBIENTSFX, HandleStop);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_AMBIENTSFX_NOW, HandleStopNow);
        }

        private void HandlePlay(FMODEvent ambientSFX)
        {
            //Is there AmbientSFX currently playing?
            FMOD.Studio.PLAYBACK_STATE playbackState;
            CurrentAmbientSFX.getPlaybackState(out playbackState);
            bool isPlaying = playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;
            //If so, wind down the old SFX, then switch in the new FMOD Event
            if (isPlaying)
            {
                if (string.IsNullOrEmpty(ambientSFX.Name))
                {
                    HandleStop();
                }
                //If it's not the same FMOD Event, then switch over
                else if (GetInstantiatedEventName(CurrentAmbientSFX) != ambientSFX.Name)
                {
                    HandleStop();
                    CurrentAmbientSFX = FMODUnity.RuntimeManager.CreateInstance(ambientSFX.Name);
                    //Handle all the parameters
                    foreach (FMODEventParameterConfig param in ambientSFX.Parameters)
                    {
                        CurrentAmbientSFX.setParameterValue(param.Name, param.Value);
                    }
                    //Actually start the FMOD Event
                    CurrentAmbientSFX.start();
                }
                //If it is the same track, just adjust the parameters
                else
                {
                    foreach (FMODEventParameterConfig param in ambientSFX.Parameters)
                    {
                        CurrentAmbientSFX.setParameterValue(param.Name, param.Value);
                    }
                }
            }
            else
            {
                CurrentAmbientSFX = FMODUnity.RuntimeManager.CreateInstance(ambientSFX.Name);
                CurrentAmbientSFX.start();
            }
        }

        private void HandleStop()
        {
            CurrentAmbientSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        private void HandleStopNow()
        {
            CurrentAmbientSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        public string GetInstantiatedEventName(FMOD.Studio.EventInstance instance)
        {
            string result;
            FMOD.Studio.EventDescription description;
            instance.getDescription(out description);
            description.getPath(out result);

            // expect the result in the form event:/folder/sub-folder/eventName
            return result;
        }
    }
}
