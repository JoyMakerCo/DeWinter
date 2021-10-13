using System;
using System.Collections.Generic;
using System.IO;
using UFlow;
using Core;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    public class SaveGameCmd : ICommand
    {
        public void Execute()
        {
            SaveSnapshot snapshot = Camera.main.GetComponent<SaveSnapshot>();
            if (snapshot != null) snapshot.TakeSnapshot(HandleSnapShot);
            else HandleSnapShot(null);
        }

        private void HandleSnapShot(byte[] snapshot)
        {
            LocalizationModel loc = AmbitionApp.GetModel<LocalizationModel>();
            GameModel model = AmbitionApp.GetModel<GameModel>();
            IncidentModel incident = AmbitionApp.GetModel<IncidentModel>();
            string path = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            List<string[]> saves;
            string result;
            string saveID = loc?.HeaderTitlePhrase;
            string snapshotPath = DateTime.Now.Ticks.ToString() + ".jpg";

            if (File.Exists(path))
            {
                result = File.ReadAllText(path);
                saves = JsonConvert.DeserializeObject<List<string[]>>(result);
            }
            else saves = new List<string[]>();

            if (!string.IsNullOrEmpty(saveID))
            {
                saveID = AmbitionApp.Localize(saveID);
                saveID = string.IsNullOrEmpty(saveID)
                    ? model.PlayerName + " - " + loc.HeaderTitlePhrase + " - " + DateTime.Now.ToString()
                    : model.PlayerName + " - " + saveID + " - " + DateTime.Now.ToString();
            }
            else saveID = model.PlayerName + " - " + DateTime.Now.ToString();

            string[] savedGame = new string[]
            {
                saveID,
                snapshotPath,
                AmbitionApp.GetService<ModelSvc>().Save()
            };
            if (snapshot != null)
            {
                snapshotPath = Path.Combine(UnityEngine.Application.persistentDataPath, snapshotPath);
                System.IO.File.WriteAllBytes(snapshotPath, snapshot);
            }
            model.SaveSlotID = saves.Count;
            saves.Add(savedGame);
            result = JsonConvert.SerializeObject(saves);
            File.WriteAllText(path, result);
        }
    }
}
