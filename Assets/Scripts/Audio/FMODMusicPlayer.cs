using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    //I know it's super kludgy to have an ambient SFX player that is extremely similar in function to the Music player, I can smooth this out later
    //The central problem is that the AudioMessagers are heard by EVERYTHING, so using any kind of generalized play audio message effects both the Music Player and the ambient SFX player

    public class FMODMusicPlayer : MonoBehaviour
    {
        private FMOD.Studio.EventInstance _playing;
        private static GameObject _instance = null;
        private IEnumerator _coroutine;

        void Awake()
        {
            if (_instance != null) Destroy(this.gameObject);
            else
            {
                _instance = this.gameObject;
                DontDestroyOnLoad(_instance);
                AmbitionApp.Subscribe<FMODEvent>(AudioMessages.PLAY_MUSIC, HandlePlay);
                AmbitionApp.Subscribe<FMODEvent>(AudioMessages.QUEUE_MUSIC, QueueNextTrack);
                AmbitionApp.Subscribe(AudioMessages.STOP_MUSIC, HandleStop);
                AmbitionApp.Subscribe(AudioMessages.STOP_MUSIC_NOW, HandleStopNow);
            }
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<FMODEvent>(AudioMessages.PLAY_AMBIENTSFX, HandlePlay);
            AmbitionApp.Unsubscribe<FMODEvent>(AudioMessages.QUEUE_MUSIC, QueueNextTrack);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_MUSIC, HandleStop);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_MUSIC_NOW, HandleStopNow);
        }

        private void HandlePlay(FMODEvent music)
        {
            if (default(FMODEvent).Equals(music)) return;
            bool instantiate = GetInstantiatedEventName(_playing) != music.Name;
            //If it's not the same FMOD Event, then switch over
            if (instantiate)
            {
                HandleStop();
                _playing = FMODUnity.RuntimeManager.CreateInstance(music.Name);
            }

            //Handle all the parameters
            foreach (FMODEventParameterConfig param in music.Parameters)
            {
                _playing.setParameterValue(param.Name, param.Value);        
            }

            //Actually start the FMOD Event
            if (instantiate) _playing.start();
        }

        private void HandleStop()
        {
            _playing.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        private void HandleStopNow()
        {
            _playing.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        private void QueueNextTrack(FMODEvent nextTrack)
        {
            _coroutine = NextTrackCheck(nextTrack);
            StartCoroutine(_coroutine); //This has to be handled in a coroutine or the check brings the whole game to a screeching halt
        }

        private IEnumerator NextTrackCheck(FMODEvent nextTrack)
        {
            //Now it's time to keep checking to see if the track has finished
            FMOD.Studio.PLAYBACK_STATE playbackState; //Have a playback state to check
            _playing.getPlaybackState(out playbackState);
            while (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                _playing.getPlaybackState(out playbackState);
                yield return null;
            }
            _playing = FMODUnity.RuntimeManager.CreateInstance(nextTrack.Name);

            //Handle all the parameters
            foreach (FMODEventParameterConfig param in nextTrack.Parameters)
            {
                _playing.setParameterValue(param.Name, param.Value);
            }

            //Actually start the FMOD Event
            _playing.start();
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
