using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
    public class GameMenuDialog : Dialog.DialogView, ISubmitHandler, IAnalogInputHandler
    {
        public Button SaveGameButton;
        public Button LoadGameButton;

        private string _path;

        public void Save()
        {
            _path = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            List<string[]> saves;
            if (File.Exists(_path))
            {
                string result = File.ReadAllText(_path);
                saves = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string[]>>(result);
                LoadGameButton.interactable = saves.Count > 0;
            }
            else saves = new List<string[]>();

            if (AmbitionApp.Game.SaveSlotID >= 0)
            {
                AmbitionApp.OpenDialog(DialogConsts.SAVE_GAME);
            }
            else if (saves.Count < AmbitionApp.Game.MaxSaves)
            {
                AmbitionApp.CloseDialog(DialogConsts.GAME_MENU);
                AmbitionApp.OpenDialog(DialogConsts.SAVE_NEW);
                AmbitionApp.SendMessage(GameMessages.SAVE_GAME);
            }
            else
            {
                AmbitionApp.OpenDialog("game_menu.saves.modal.too_many_saves", null, null);
            }
        }

        public void Cancel() => Close();
        public void Submit() { }

        public void HandleInput(UnityEngine.Vector2 [] input)
        {
        }

        public void ResetGame()
        {
            AmbitionApp.OpenDialog("exit", () => AmbitionApp.SendMessage(GameMessages.EXIT_GAME));
        }

        private void Start()
        {
            _path = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            if (File.Exists(_path))
            {
                string result = File.ReadAllText(_path);
                List<string[]>  saves = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string[]>>(result);
                LoadGameButton.interactable = saves.Count > 0;
            }
            else
            {
                LoadGameButton.interactable = false;
            }
            SaveGameButton.interactable = !AmbitionApp.Story.IsComplete(GameMessages.EPILOGUE, false);
        }
    }
}
