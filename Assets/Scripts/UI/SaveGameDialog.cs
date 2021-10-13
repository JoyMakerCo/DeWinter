using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Dialog;
namespace Ambition
{
    public class SaveGameDialog : DialogView, ISubmitHandler, IAnalogInputHandler
    {
        public List<SavedGameListItem> Pool;
        public Button LoadGameButton;
        public ToggleGroup SaveStateGroup;
        public Image Snapshot;
        public Text TitleText;
        public Text FileCountTxt;

        private SavedGameListItem _selected;
        private List<string[]> _saves;
        private bool _dirty = false;
        private int _index;
        private Color _defaultCountTextColor = default;

        public override void OnOpen()
        {
            string filepath = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            Transform parent = Pool[0].transform.parent;
            GameObject obj;
            int poolCount = Pool.Count;
            int p = 0;
            List<string> snapshots = new List<string>(Directory.GetFiles(UnityEngine.Application.persistentDataPath));
            snapshots.RemoveAll(s => !s.EndsWith(".jpg"));

            if (File.Exists(filepath))
            {
                string result = File.ReadAllText(filepath);
                _saves = JsonConvert.DeserializeObject<List<string[]>>(result);
            }
            _saves ??= new List<string[]>();
            FileCountTxt.text = _saves.Count + "/" + AmbitionApp.Game.MaxSaves;
            _defaultCountTextColor = FileCountTxt.color;
            if (_saves.Count > AmbitionApp.Game.MaxSaves)
                FileCountTxt.color = Color.red;

            // the last item is the most recent; list that at the top
            for (int i=_saves.Count-1; i>=0; --i)
            {
                if (p < poolCount) Pool[p++].SetData(_saves[i]);
                else
                {
                    obj = Instantiate(Pool[0].gameObject, parent);
                    _selected = obj.GetComponent<SavedGameListItem>();
                    _selected.SetData(_saves[i]);
                    Pool.Add(_selected);
                }
                snapshots.Remove(Path.Combine(UnityEngine.Application.persistentDataPath, _saves[i][1]));
            }
            for (int i=_saves.Count; i<poolCount; ++i)
            {
                Pool[i].gameObject.SetActive(false);
            }
            foreach(string snapshot in snapshots)
            {
                filepath = Path.Combine(UnityEngine.Application.persistentDataPath, snapshot);
                File.Delete(filepath);
            }
            if (!string.IsNullOrEmpty(Pool[0].Data))
            {
                SelectState(Pool[0]);
            }
        }

        public void SelectState(SavedGameListItem item)
        {
            if (_selected != null) _selected.Selected = false;
            _selected = item;
            _selected.Selected = true;
            string filepath = Path.Combine(UnityEngine.Application.persistentDataPath, _selected.Snapshot);
            if (File.Exists(filepath))
            {
                byte[] bytes = File.ReadAllBytes(filepath);
                Texture2D tex = new Texture2D(1, 1);
                Snapshot.gameObject.SetActive(true);
                tex.LoadImage(bytes);
                Snapshot.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
                Snapshot.preserveAspect = true;
            }
            else
            {
                Snapshot.gameObject.SetActive(false);
            }
            LoadGameButton.interactable = true;
            TitleText.text = _selected.SaveText.text;

            if (this.isActiveAndEnabled)
               StartCoroutine(ContentFitterHax());

            _index = Pool.Count - Pool.IndexOf(_selected) - 1;
        }

        public void LoadSelected()
        {
            if (_selected != null)
            {
                AmbitionApp.OpenDialog("restore", OnConfirm, new Dictionary<string, string>() { { "%g", _selected.SaveText.text } });
            }
        }

        public void DeleteSelected()
        {
            if (_selected != null)
            {
                AmbitionApp.OpenDialog("delete_saved", OnDelete, new Dictionary<string, string>() { { "%g", _selected.SaveText.text } });
            }
        }

        public override void OnClose()
        {
            if (_dirty)
            {
                string filepath = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
                string result = JsonConvert.SerializeObject(_saves);
                File.WriteAllText(filepath, result);
            }
        }

        public void Submit() => LoadSelected();
        public void Cancel() => Close();
        public void HandleInput(Vector2 [] input)
        {
        }

        private void OnConfirm()
        {
            AmbitionApp.SendMessage(GameMessages.LOAD_GAME, _selected.Data);
            AmbitionApp.Game.SaveSlotID = _index;
        }

        private void OnDelete()
        {
            if (_selected != null)
            {
                if (_index >= 0)
                    _saves.RemoveAt(_index);
                if (_index == AmbitionApp.Game.SaveSlotID)
                    AmbitionApp.Game.SaveSlotID = -1;
                _dirty = true;
                _index = -1;
                Destroy(_selected.gameObject);
                SaveStateGroup.SetAllTogglesOff(true);
                TitleText.text = "";
                LoadGameButton.interactable = false;
                Snapshot.gameObject.SetActive(false);
                _selected = null;

                FileCountTxt.text = _saves.Count + "/" + AmbitionApp.Game.MaxSaves;
                FileCountTxt.color = (_saves.Count > AmbitionApp.Game.MaxSaves)
                     ? Color.red
                     : _defaultCountTextColor;
            }
        }

        System.Collections.IEnumerator ContentFitterHax()
        {
            yield return new UnityEngine.WaitForEndOfFrame();
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(TitleText.transform.parent as RectTransform);
        }
    }
}
