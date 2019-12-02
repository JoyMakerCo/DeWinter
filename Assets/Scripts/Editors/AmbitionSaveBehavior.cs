#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using Ambition;

namespace AmbitionEditor
{
    public class AmbitionSaveBehavior : AssetModificationProcessor
    {
        private static string[] OnWillSaveAssets(string [] assets)
        {
            Type t = typeof(ILocalizedAsset);
            Dictionary<string, string> localizations = new Dictionary<string, string>();
            List<string> remove = new List<string>();
            Dictionary<string, string> currLocs;
            List<string> currRemove;
            foreach (string asset in assets)
            {
                ILocalizedAsset localizedAsset = AssetDatabase.LoadAssetAtPath(asset, t) as ILocalizedAsset;
                LocalizationConfig.UpdateLocalizedAsset(localizedAsset, out currLocs, out currRemove);
                remove.AddRange(currRemove);
                foreach(string k in currLocs.Keys)
                {
                    localizations[k] = currLocs[k];
                }
            }
            LocalizationConfig.Post(localizations, remove);
            LocalizationConfig.WriteToFile();
            return assets;
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
