using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
namespace Ambition
{
    public class AmbitionErrorCmd : Core.ICommand<ErrorEventArgs>
    {
        public void Execute(ErrorEventArgs args)
        {
            switch (args.Type)
            {
                case ErrorEventArgs.ErrorType.save_file:
                    HandleSaveFileError(args.Data[0]);
                    break;
            }
            AmbitionApp.OpenDialog(DialogConsts.ERROR, args);
        }

        private void HandleSaveFileError(string filepath)
        {
            if (File.Exists(filepath))
            {
                string newPath = filepath;
                int index = newPath.LastIndexOf('.');
                if (index < 0) newPath += "BACKUP";
                else newPath = newPath.Insert(index, "BACKUP");
                if (File.Exists(newPath)) File.Delete(newPath);
                File.Move(filepath, newPath);

                string autosaveFile = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.AUTOSAVE);
                string[][] saves = new string[1][] { new string[]{ "Autosave", "", File.ReadAllText(autosaveFile) } };
                File.WriteAllText(filepath, Newtonsoft.Json.JsonConvert.SerializeObject(saves));
            }
        }
    }
}
