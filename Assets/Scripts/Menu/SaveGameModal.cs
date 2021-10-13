using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core;
using Dialog;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Ambition
{
    public class SaveGameModal : DialogView, ISubmitHandler
    {
        private const string OLD_SAVE_NAME = "$OLDSAVENAME";
        private const string OVERWRITE_PHRASE = "game_menu.saves.overwrite_save.body";

        public Button CancelBtn;
        public Button OverwriteBtn;
        public Button CreateBtn;
        public Text SaveTxt;
        public Text StatusTxt;

        private List<string[]> _saves;
        private string _path;

        public override void OnOpen()
        {
            _path = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            if (File.Exists(_path))
            {
                string result = File.ReadAllText(_path);
                Dictionary<string, string> subs = new Dictionary<string, string>();
                int slotID = AmbitionApp.Game.SaveSlotID;
                _saves = JsonConvert.DeserializeObject<List<string[]>>(result);
                if (slotID >= 0 && slotID < _saves.Count)
                {
                    subs[OLD_SAVE_NAME] = _saves[slotID][0];
                }
                SaveTxt.text = AmbitionApp.Localize(OVERWRITE_PHRASE, subs);
                StatusTxt.enabled = _saves.Count >= AmbitionApp.Game.MaxSaves;
                CreateBtn.interactable = !StatusTxt.enabled;
                OverwriteBtn.interactable = slotID >= 0;
            }
            else _path = null;
        }

        public void OnCreate() => StartCoroutine(SaveGameDelay());
        public void OnOverwrite()
        {
            if (_path != null)
            {
                int index = AmbitionApp.Game.SaveSlotID;
                string result;
                if (index < _saves.Count)
                {
                    result = Path.Combine(Application.persistentDataPath, _saves[index][1]);
                    File.Delete(result);
                    _saves.RemoveAt(index);
                }
                result = JsonConvert.SerializeObject(_saves);
                File.WriteAllText(_path, result);
                AmbitionApp.Game.SaveSlotID = _saves.Count;
                StartCoroutine(SaveGameDelay());
            }
        }

        public void Cancel() => Close();
        public void Submit()
        {
            if (OverwriteBtn.interactable)
                OnOverwrite();
            else if (CreateBtn.interactable)
                OnCreate();
        }

        IEnumerator SaveGameDelay()
        {
            CancelBtn.interactable = false;
            OverwriteBtn.interactable = false;
            CreateBtn.interactable = false;
            StatusTxt.text = AmbitionApp.Localize("game_menu.saving");
            StatusTxt.enabled = true;
            AmbitionApp.SendMessage(GameMessages.SAVE_GAME);
            yield return new WaitForSeconds(1f);
            Close();
            AmbitionApp.CloseDialog(DialogConsts.GAME_MENU);
        }
    }
}
