using System;
using System.IO;
using Newtonsoft.Json;
namespace Ambition
{
    public class ContinueGameCmd : Core.ICommand
    {
        public void Execute()
        {
            string autosave = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.AUTOSAVE);
            if (File.Exists(autosave))
            {
                autosave = File.ReadAllText(autosave);
                if (!string.IsNullOrEmpty(autosave))
                {
                    AmbitionApp.SendMessage(GameMessages.LOAD_GAME, autosave);
                }
            }
        }
    }
}
