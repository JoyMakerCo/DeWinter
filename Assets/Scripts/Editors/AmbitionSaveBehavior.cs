#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Ambition;

namespace AmbitionEditor
{
    public class AmbitionSaveBehavior : UnityEditor.AssetModificationProcessor
    {

        private static string[] OnWillSaveAssets(string [] paths)
        {
            string[] files = AssetDatabase.FindAssets("t:" + typeof(LocalizationManager).Name);
            if (files.Length > 0)
            {
                files[0] = AssetDatabase.GUIDToAssetPath(files[0]);
                LocalizationManager localizer = AssetDatabase.LoadAssetAtPath<LocalizationManager>(files[0]);
                localizer?.AddFiles(paths);
            }
            return paths;
        }

/* "OnWillMoveAssets" doesn't work as intended; Renaming/moving a file will require an explicit save
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            ILocalizedAsset obj = AssetDatabase.LoadAssetAtPath(sourcePath, typeof(ILocalizedAsset)) as ILocalizedAsset;
            if (obj == null) return AssetMoveResult.DidMove;
            Dictionary<string, string> locs = new Dictionary<string, string>();
            List<string> remove = new List<string>();
            if (LocalizationConfig.UpdateLocalizedAsset(obj, out locs, out remove) == null)
            {
                return AssetMoveResult.FailedMove;
            }
            LocalizationConfig.Post(locs, remove);
            LocalizationConfig.WriteToFile();
            return AssetMoveResult.DidMove;
        }
*/
    }
}
#endif
