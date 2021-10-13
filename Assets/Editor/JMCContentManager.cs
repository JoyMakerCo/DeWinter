using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Ambition
{
    public class JMCContentManager
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Nuke Local Files")]
        public static void NukeFiles()
        {
            string[] files = Directory.GetFiles(UnityEngine.Application.persistentDataPath);
            foreach (var file in files)
            {
                new FileInfo(file).Delete();
            }
        }
#endif
    }
}
