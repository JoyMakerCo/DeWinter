#if DEBUG
using System;
using UnityEngine;
using UnityEditor;
namespace Ambition
{
    public class LocalizeAssetsCmd : Core.ICommand
    {
        public void Execute()
        {
            string[] guids = AssetDatabase.FindAssets("t:AmbitionEditor.LocalizationConfig");
            string path;
            foreach (string guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
                AssetDatabase.LoadAssetAtPath<AmbitionEditor.LocalizationConfig>(path)?.CreateDefaultLocalizations();
            }
        }
    }
}
#endif
