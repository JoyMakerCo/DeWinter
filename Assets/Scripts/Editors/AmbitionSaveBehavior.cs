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
        private static string[] OnWillSaveAssets(string [] assets)
        {
            Type t = typeof(ILocalizedAsset);
            Dictionary<string, string> dictionary = LocalizationConfig.GetPhrases();
            if (dictionary != null)
            {
                Dictionary<string, string> localizations = new Dictionary<string, string>();
                Dictionary<string, string> assetLocalizations;
                List<string> removals = new List<string>();
                ILocalizedAsset localizedAsset;
                string locKey;
                IEnumerable<string> tempList;
                foreach (string asset in assets)
                {
                    localizedAsset = AssetDatabase.LoadAssetAtPath(asset, t) as ILocalizedAsset;
                    if (localizedAsset != null)
                    {
                        locKey = AssetDatabase.GetAssetPath(localizedAsset as UnityEngine.Object);
                        assetLocalizations = localizedAsset.Localize();
                        removals.AddRange(dictionary.Keys.Where(k => k.StartsWith(localizedAsset.LocalizationKey)));
                        tempList = assetLocalizations.Keys;

                        localizedAsset.LocalizationKey = locKey;

                        // Move over the list of localizations in the object
                        // This will stomp old values
                        foreach (string k in tempList)
                        {
                            if (!string.IsNullOrEmpty(assetLocalizations[k] as string))
                            {
                                string key = (k != "") ? (locKey + "." + k) : locKey;
                                localizations[key] = assetLocalizations[k];
                                removals.Remove(key);
                            }
                        }
                    }
                }
                LocalizationConfig.Post(localizations, removals);
            }

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
