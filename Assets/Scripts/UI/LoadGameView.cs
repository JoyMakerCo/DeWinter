using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Core;
using Newtonsoft.Json;
namespace Ambition
{
    public class LoadGameView : MonoBehaviour
    {
        public SavedGameListItem ListItem;

        public void PopulateDialog()
        {
            string filepath = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            List<string[]> saves;
            int count = 0;
            SavedGameListItem item = ListItem;
            GameObject obj;
            Transform parent = ListItem.transform.parent;
            if(File.Exists(filepath))
            {
                string result = File.ReadAllText(filepath);
                saves = JsonConvert.DeserializeObject<List<string[]>>(result);
            }
            else
            {
                saves = new List<string[]>();
            }
            ListItem.gameObject.SetActive(saves.Count > 0);
            foreach (string[] save in saves)
            {
                item.SaveText.text = save[0];
                item.Data = save[2];
                if (++count < saves.Count)
                {
                    obj = Instantiate<GameObject>(ListItem.gameObject, parent);
                    obj.transform.SetSiblingIndex(0);
                    item = obj.GetComponent<SavedGameListItem>();
                }
            }
        }

        private void OnDisable()
        {
            Transform xf;
            for (int i= ListItem.transform.parent.childCount-1; i>0; --i)
            {
                xf = ListItem.transform.parent.GetChild(i);
                Destroy(xf.gameObject);
            }
        }
    }
}
