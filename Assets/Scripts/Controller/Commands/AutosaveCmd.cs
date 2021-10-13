using System;
using System.IO;
using Newtonsoft.Json;
namespace Ambition
{
    public class AutosaveCmd : Core.ICommand
    {
        public void Execute()
        {
            if (AmbitionApp.Game.Activity != ActivityType.Title && !AmbitionApp.Story.IsComplete(SceneConsts.EPILOGUE_SCENE, false))
            {
                string path = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.AUTOSAVE);
                string autosave = AmbitionApp.GetService<Core.ModelSvc>().Save();
                File.WriteAllText(path, autosave);
            }
        }
    }
}
