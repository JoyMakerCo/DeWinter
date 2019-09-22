#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace Ambition
{
    public class AmbitionPostprocessor// : AssetPostprocessor
    {
        private const string PATH_SOURCE = "SourceBundles/";
        private const string PATH_OUT = "Assets/AssetBundles";


//        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
//        {
//            if (!Directory.Exists(PATH_OUT))
//            {
//                Directory.CreateDirectory(PATH_OUT);
//            }

//            List<string> repack = new List<string>();
//            UpdateList(importedAssets, repack);
//            UpdateList(deletedAssets, repack);
//            UpdateList(movedAssets, repack);
//            UpdateList(movedFromPath, repack);

//            foreach (string path in repack)
//            {
//Debug.Log("Repack" + path);
        //        //BuildPipeline.BuildAssetBundles(PATH_OUT + path, BuildAssetBundleOptions.None, 
        //    }
        //}

        private static void UpdateList(string[] assets, List<string> result)
        {
            // Find the folder path of the modified asset
            // Add the folder path if it's not already included
        }
    }
}
#endif