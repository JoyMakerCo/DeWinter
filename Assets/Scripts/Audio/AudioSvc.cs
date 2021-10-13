using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Core;

namespace Ambition
{
    public class AudioSvc : IAppService
    {
        public bool IsLoading => RuntimeManager.AnyBankLoading();

        private const string SFX_VOLUME = "vca:/SFX ONE SHOTS";
        private const string AMBIENT_VOLUME = "vca:/SFX AMBIENT";
        private const string MUSIC_VOLUME = "vca:/MUSIC";
        private const string MASTER_VOLUME = "vca:/MASTER";

        private Dictionary<AudioChannel, int> _volume = new Dictionary<AudioChannel, int>();
        private Dictionary<AudioChannel, EventInstance> _channels;
        private List<string> _banks = new List<string>();
        private float _master = 1f;

        private string GetEventID(AudioChannel channel)
        {
            _channels[channel].getPlaybackState(out PLAYBACK_STATE state);
            if (state == PLAYBACK_STATE.STOPPED) return null;

            _channels[channel].getDescription(out EventDescription d);
            d.getPath(out string s);
            return s;
        }

        public void Load(List<string> banks)
        {
            foreach (string bank in banks)
            {
                if (!_banks.Contains(bank)) _banks.Add(bank);
            }
        }

        public void SetVolume(AudioChannel channel, int volume)
        {
            _volume[channel] = volume;
            switch(channel)
            {
                case AudioChannel.Sfx:
                    RuntimeManager.GetVCA(SFX_VOLUME).setVolume(_master * .01f * volume);
                    break;
                case AudioChannel.Music:
                    RuntimeManager.GetVCA(MUSIC_VOLUME).setVolume(_master * .01f * volume);
                    break;
                case AudioChannel.Ambient:
                    RuntimeManager.GetVCA(AMBIENT_VOLUME).setVolume(_master * .01f * volume);
                    break;
                default:
                    VCA vca;
                    _master = .01f * volume;
                    if (RuntimeManager.StudioSystem.getVCA(SFX_VOLUME, out vca) == FMOD.RESULT.OK && _volume.TryGetValue(AudioChannel.Sfx, out volume))
                        vca.setVolume(_master * .01f * volume);
                    if (RuntimeManager.StudioSystem.getVCA(MUSIC_VOLUME, out vca) == FMOD.RESULT.OK && _volume.TryGetValue(AudioChannel.Music, out volume))
                        vca.setVolume(_master * .01f * volume);
                    if (RuntimeManager.StudioSystem.getVCA(AMBIENT_VOLUME, out vca) == FMOD.RESULT.OK && _volume.TryGetValue(AudioChannel.Ambient, out volume))
                        vca.setVolume(_master * .01f * volume);
                    if (RuntimeManager.StudioSystem.getVCA(MASTER_VOLUME, out vca) == FMOD.RESULT.OK && _volume.TryGetValue(AudioChannel.Master, out volume))
                        vca.setVolume(_master * .01f * volume);
                    break;
            }

        }

        public int GetVolume(AudioChannel channel) => _volume.TryGetValue(channel, out int volume) ? volume : 100;

        public AudioSvc()
        {
            _banks = new List<string>();
            _channels = new Dictionary<AudioChannel, EventInstance>()
            {
                {AudioChannel.Sfx, new EventInstance()},
                {AudioChannel.Music, new EventInstance()},
                {AudioChannel.Ambient, new EventInstance()}
            };

            AmbitionApp.Subscribe<FMODEvent>(AudioMessages.PLAY, HandlePlaySound);
            AmbitionApp.Subscribe<string>(AudioMessages.PLAY, HandlePlaySound);
            AmbitionApp.Subscribe<FMODEvent>(AudioMessages.PLAY_AMBIENT, HandlePlayAmbient);    
            AmbitionApp.Subscribe(AudioMessages.STOP_AMBIENT, HandleStopAmbient);    
            AmbitionApp.Subscribe(AudioMessages.STOP_AMBIENT_NOW, HandleStopAmbientNow);    
            AmbitionApp.Subscribe<FMODEvent>(AudioMessages.PLAY_MUSIC, HandlePlayMusic);    
            AmbitionApp.Subscribe(AudioMessages.STOP_MUSIC, HandleStopMusic);    
            AmbitionApp.Subscribe(AudioMessages.STOP_MUSIC_NOW, HandleStopMusicNow);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<FMODEvent>(AudioMessages.PLAY, HandlePlaySound);
            AmbitionApp.Unsubscribe<FMODEvent>(AudioMessages.PLAY_AMBIENT, HandlePlayAmbient);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_AMBIENT, HandleStopAmbient);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_AMBIENT_NOW, HandleStopAmbientNow);
            AmbitionApp.Unsubscribe<FMODEvent>(AudioMessages.PLAY_MUSIC, HandlePlayMusic);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_MUSIC, HandleStopMusic);
            AmbitionApp.Unsubscribe(AudioMessages.STOP_MUSIC_NOW, HandleStopMusicNow);
            _banks.ForEach(RuntimeManager.UnloadBank);
        }

        private void HandlePlaySound(FMODEvent sound) => RuntimeManager.PlayOneShot(sound.Name);
        private void HandlePlaySound(string soundID) => RuntimeManager.PlayOneShot("event:/" + soundID);
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
            if (GetEventID(channel) == sound.Name)
            {
                _channels[channel].getPlaybackState(out PLAYBACK_STATE state);
                if (state == PLAYBACK_STATE.STOPPING) Stop(channel, false);
                else
                {
                    Array.ForEach(sound.Parameters, p => _channels[channel].setParameterByName(p.Name, p.Value));
                    return;
                }
            }
            else Stop(channel);
            if (!string.IsNullOrWhiteSpace(sound.Name))
            {
                _channels[channel] = FMODUnity.RuntimeManager.CreateInstance(sound.Name);
                _channels[channel].start();
                Array.ForEach(sound.Parameters, p => _channels[channel].setParameterByName(p.Name, p.Value));
            }
        }
    }
}
