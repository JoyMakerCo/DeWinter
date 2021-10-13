using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json;

namespace Ambition
{
    public class StartScene : SceneView, Util.IInitializable, IDisposable, IAnalogInputHandler, ISubmitHandler
    {
        private const float SELECT_DELAY = .1f;

        public Button ContinueGameBtn;
        public Button LoadGameBtn;
        public Button[] Buttons;
        public FMODEvent StartupSound;
        public FMODEvent ClickSound;
        public string[] Players;

        private int _index = -1;
        private float _increment = 0;
        private bool _selecting = false;

        public void Initialize()
        {
            string saveFile = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            string _autosavefile = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.AUTOSAVE);
            AmbitionApp.Game.Activity = ActivityType.Title;
            if (File.Exists(saveFile))
            {
                string result = File.ReadAllText(saveFile);
                List<string[]> saves = JsonConvert.DeserializeObject<List<string[]>>(result);
                if (saves == null)
                {
                    ErrorEventArgs error = new ErrorEventArgs(ErrorEventArgs.ErrorType.save_file, saveFile);
                    AmbitionApp.SendMessage(GameMessages.ERROR, error);
                    LoadGameBtn.gameObject.SetActive(File.Exists(saveFile));
                }
                else LoadGameBtn.gameObject.SetActive(saves.Count > 0);
            }
            else LoadGameBtn.gameObject.SetActive(false);
            ContinueGameBtn?.gameObject.SetActive(File.Exists(_autosavefile));
            AmbitionApp.Subscribe<float>(GameMessages.FADE_OUT, HandleFade);
            AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, StartupSound);
        }

        public void Continue()
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY, ClickSound);
            AmbitionApp.Execute<ContinueGameCmd>();
        }

        public void NewGame()
        {
            AmbitionApp.Execute<NewGameCmd, string>(Players[0]);
        }

        public void LoadGame() => AmbitionApp.OpenDialog(DialogConsts.LOAD_GAME);
        public void ShowSettings() => AmbitionApp.OpenDialog(DialogConsts.GAME_OPTIONS);

        public void HandleInput(Vector2 [] sticks)
        {
            float increment = 0;
            Array.ForEach(sticks, s => increment += s.x);
            _selecting = increment != 0;
            if (!_selecting) _increment = 0;
            else
            {
                int index = _index < 0 ? (_index = 0) : _index;
                int count = Buttons.Length;
                if (increment < -1) _increment -= Time.deltaTime;
                else if (increment > 1) _increment += Time.deltaTime;
                else _increment += increment * Time.deltaTime;
                if (_increment > SELECT_DELAY)
                {
                    index = (index + 1) % count;
                    _increment -= SELECT_DELAY;
                }
                else if (_increment < -SELECT_DELAY)
                {
                    --index;
                    while (index < 0) index += count;
                    _increment += SELECT_DELAY;
                }
                while (index != _index && !Buttons[index].gameObject.activeInHierarchy)
                {
                    index = _increment > 0
                        ? (index + 1)%count
                        : index-1;
                    while (index < count) index += count;
                }
                if (index != _index)
                {
                    _index = index;
                    EventSystem.current.SetSelectedGameObject(Buttons[_index].gameObject);
                }
            }
        }

        public void Cancel() => LoadGame();
        public void Submit()
        {
            if (_index >= 0)
            {
                Buttons[_index].onClick.Invoke();
            }
            else if (ContinueGameBtn.isActiveAndEnabled)
            {
                Continue();
            }
            else
            {
                NewGame();
            }
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<float>(GameMessages.FADE_OUT, HandleFade);
        }

        private void HandleFade(float time) => AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
    }
}
