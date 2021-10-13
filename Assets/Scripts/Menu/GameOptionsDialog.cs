using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;
using Dialog;
using System.IO;
using Newtonsoft.Json;

namespace Ambition
{
    public class GameOptionsDialog : DialogView, ISubmitHandler, IAnalogInputHandler
    {
        public Dropdown LocSelector;
        public Dropdown ResolutionSelector;
        public FontSet Fonts;
        public Text LocTxt;
        public Text FullscreenBtnLabel;

        private LocalizationSvc _svc;
        private List<SystemLanguage> _languages;
        private GameSettingsVO _settings;
        private Dictionary<SystemLanguage, TextAsset> _locs;
        private SystemLanguage _language = SystemLanguage.Unknown;
        private bool _isFullscreen;

        public override void OnOpen()
        {
            string filepath = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SETTINGS);
            SystemLanguage language;
            List<string> languages = new List<string>();

            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);
                _settings = JsonConvert.DeserializeObject<GameSettingsVO>(json);
            }
            else
            {
                _settings = new GameSettingsVO() { Resolution = Screen.currentResolution };
            }

            _svc = AmbitionApp.GetService<LocalizationSvc>();
            _locs = new Dictionary<SystemLanguage, TextAsset>();
            TextAsset[] locs = Resources.LoadAll<TextAsset>(Filepath.LOCALIZATIONS);
            _languages = new List<SystemLanguage>() { _settings.Language };
            languages.Add(_settings.Language.ToString());

            LocSelector.ClearOptions();
            foreach (TextAsset asset in locs)
            {
                if (Enum.TryParse(asset.name, out language))
                {
                    _locs[language] = asset;
                    if (language != _svc.DefaultLanguage)
                    {
                        _languages.Add(language);
                        languages.Add(language.ToString());
                    }
                }
            }
            LocSelector.AddOptions(languages);
            LocSelector.value = 0;
            LocSelector.captionText.text = locs[0].name;
            HandleLoc();

            int index = 0;
            List<string> options = new List<string>();
            for (int i = 0; i < GameSettingsVO.RESOLUTIONS.Length; ++i)
            {
                if (GameSettingsVO.RESOLUTIONS[i] >= Screen.width) index = (i>>1);
                options.Add(GameSettingsVO.RESOLUTIONS[i] + " x " + GameSettingsVO.RESOLUTIONS[++i]);
            }

            ResolutionSelector.ClearOptions();
            ResolutionSelector.AddOptions(options);
            ResolutionSelector.value = index;
            ResolutionSelector.RefreshShownValue();
            FullscreenBtnLabel.text = AmbitionApp.Localize(Screen.fullScreen ? "options.windowed" : "options.fullscreen");
        }

        public void Submit() => Close();
        public void Cancel() => Close();

        public void HandleInput(Vector2 [] input)
        {
            foreach(Vector2 stick in input)
            {
            }
        }

        public void HandleLoc()
        {
            if (_language != _languages[LocSelector.value])
            {
                AmbitionApp.SendMessage(GameMessages.UPDATE_LOCALIZATION, _settings.Language);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(ListenForFullscreenChange());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public override void OnClose()
        {
            AudioSvc audio = AmbitionApp.GetService<AudioSvc>();
            string path = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SETTINGS);
            _settings.Volume = new Dictionary<AudioChannel, int>()
            {
                {AudioChannel.Ambient,audio.GetVolume(AudioChannel.Ambient)},
                {AudioChannel.Sfx,audio.GetVolume(AudioChannel.Sfx)},
                {AudioChannel.Music,audio.GetVolume(AudioChannel.Music)},
                {AudioChannel.Master,audio.GetVolume(AudioChannel.Master)}
            };
            _settings.Resolution = Screen.currentResolution;
            _settings.Language = _languages[LocSelector.value];
            File.WriteAllText(path, JsonConvert.SerializeObject(_settings));
        }

        public void ToggleFullscreen()
        {
            _settings.Fullscreen = !_settings.Fullscreen;
            Screen.fullScreenMode = _settings.Fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }

        // Not used, but useful to keep around
        IEnumerator SetVolume(AudioChannel channel, int delta)
        {
            AudioSvc audio = AmbitionApp.GetService<AudioSvc>();
            int volume = audio.GetVolume(channel);
            do
            {
                volume += delta;
                if (volume < 0) volume = 0;
                else if (volume > 100) volume = 100;
                audio.SetVolume(channel, volume);
                _settings.Volume[channel] = volume;
                yield return null;
            }
            while (volume >= 0 && volume <= 100);
            _settings.Volume[channel] = volume;
        }

        public void HandleResolution()
        {
            Resolution rez = _settings.Resolution = new Resolution()
            {
                width = GameSettingsVO.RESOLUTIONS[ResolutionSelector.value * 2],
                height = GameSettingsVO.RESOLUTIONS[ResolutionSelector.value * 2 + 1]
            };
            if (Screen.width != rez.width || Screen.height != rez.height)
                Screen.SetResolution(rez.width, rez.height, Screen.fullScreen, Screen.currentResolution.refreshRate);
        }

        IEnumerator ListenForFullscreenChange()
        {
            while(true)
            {
                if (Screen.fullScreen != _isFullscreen)
                {
                    _isFullscreen = Screen.fullScreen;
                    FullscreenBtnLabel.text = AmbitionApp.Localize(_isFullscreen ? "options.windowed" : "options.fullscreen");
                }
                yield return null;
            }
        }
    }
}
