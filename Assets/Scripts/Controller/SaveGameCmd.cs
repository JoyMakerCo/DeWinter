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
            string path = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            GameModel model = AmbitionApp.GetModel<GameModel>();
            string saveID = model.PlayerName + " " + DateTime.Now.ToString();
            string result;
            List<string[]> saves;
            if (File.Exists(path))
            {
                result = File.ReadAllText(path);
                saves = JsonConvert.DeserializeObject<List<string[]>>(result);
            }
            else
            {
                saves = new List<string[]>(); 
            }
            string[] savedGame = new string[] { saveID, DateTime.Now.Ticks.ToString(), AmbitionApp.GetService<ModelSvc>().Save() };
            saves.Add(savedGame);
            result = JsonConvert.SerializeObject(saves);
            File.WriteAllText(path, result);
        }
    }
}
