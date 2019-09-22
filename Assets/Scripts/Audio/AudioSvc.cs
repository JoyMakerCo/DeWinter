using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Core;

namespace Ambition
{
    public class AudioSvc : IAppService
    {
        private Dictionary<AudioChannel, EventInstance> _channels;
        private List<string> _banks = new List<string>();

        private string GetEventID(AudioChannel channel)
        {
            _channels[channel].getPlaybackState(out PLAYBACK_STATE state);
            if (state == PLAYBACK_STATE.STOPPED) return null;

            _channels[channel].getDescription(out EventDescription d);
            d.getPath(out string s);
            return s;
        }

        public void Load(List<string> banks) => _banks.AddRange(banks);

        public AudioSvc()
        {
            _banks = new List<string>();
            _channels = new Dictionary<AudioChannel, EventInstance>()
            {
                {AudioChannel.Ambient, new EventInstance()},
                {AudioChannel.Music, new EventInstance()}
            };
    
            AmbitionApp.Subscribe<FMODEvent>(AudioMessages.PLAY_AMBIENT, HandlePlayAmbient);    
            AmbitionApp.Subscribe(AudioMessages.STOP_AMBIENT, HandleStopAmbient);    
            AmbitionApp.Subscribe(AudioMessages.STOP_AMBIENT_NOW, HandleStopAmbientNow);    
            AmbitionApp.Subscribe<FMODEvent>(AudioMessages.PLAY_MUSIC, HandlePlayMusic);    
            AmbitionApp.Subscribe(AudioMessages.STOP_MUSIC, HandleStopMusic);    
            AmbitionApp.Subscribe(AudioMessages.STOP_MUSIC_NOW, HandleStopMusicNow);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<FMODEvent>(AudioMessages.PLAY_AMBIENT, HandlePlayAmbient);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_AMBIENT, HandleStopAmbient);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_AMBIENT_NOW, HandleStopAmbientNow);
            AmbitionApp.Unsubscribe<FMODEvent>(AudioMessages.PLAY_MUSIC, HandlePlayMusic);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_MUSIC, HandleStopMusic);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_MUSIC_NOW, HandleStopMusicNow);
            _banks.ForEach(RuntimeManager.UnloadBank);
        }

        private void HandlePlayAmbient(FMODEvent e) => Play(e, AudioChannel.Ambient);
        private void HandleStopAmbient() => Stop(AudioChannel.Ambient);
        private void HandleStopAmbientNow() => Stop(AudioChannel.Ambient, false);
        private void HandlePlayMusic(FMODEvent e) => Play(e, AudioChannel.Music);
        private void HandleStopMusic() => Stop(AudioChannel.Music);
        private void HandleStopMusicNow() => Stop(AudioChannel.Music, false);

        private void Stop(AudioChannel channel, bool fadeout=true)
        {
            _channels[channel].stop(fadeout
                ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT
                : FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        private void Play(FMODEvent sound, AudioChannel channel)
        {
            if (GetEventID(channel) != sound.Name)
            {
                Stop(channel);
                if (!string.IsNullOrWhiteSpace(sound.Name))
                {
                    _channels[channel] = FMODUnity.RuntimeManager.CreateInstance(sound.Name);
                    _channels[channel].start();
                }
            }
            Array.ForEach(sound.Parameters, p => _channels[channel].setParameterValue(p.Name, p.Value));
        }
    }
}
