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
    }
}
#endif
