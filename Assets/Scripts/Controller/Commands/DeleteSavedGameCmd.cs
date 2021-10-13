using System;
using System.Collections.Generic;
using System.IO;
using Core;
using Newtonsoft.Json;
namespace Ambition
{
    public class DeleteSavedGameCmd : ICommand<string>
    {
        public void Execute(string saveID)
        {
            string path = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            if (File.Exists(path))
            {
                string result = File.ReadAllText(path);
                List<string[]> saves = JsonConvert.DeserializeObject<List<string[]>>(result);
                for (int i=saves.Count;  i>=0;  --i)
                {
                    if (saves[i][0] == saveID)
                    {
                        saves.RemoveAt(i);
                        result = JsonConvert.SerializeObject(saves);
                        File.WriteAllText(path, result);
                        return;
                    }
                }
            }
        }
    }
}
