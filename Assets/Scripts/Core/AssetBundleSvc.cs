using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core
{
    //TODO: Android and WebGL use UnityWebRequest instead of StreamingAssetsPath.
    public class AssetBundleSvc : IAppService
    {
        protected Dictionary<string, AssetBundle> _bundles = new Dictionary<string, AssetBundle>();

        public void Load(string assetBundleID, Action<AssetBundle> OnLoaded)
        {
            if (_bundles.TryGetValue(System.IO.Path.Combine(UnityEngine.Application.streamingAssetsPath, assetBundleID), out AssetBundle bundle))
                OnLoaded(bundle);
            else
            {
                bundle = AssetBundle.LoadFromFile(Path.Combine(UnityEngine.Application.streamingAssetsPath, assetBundleID));
                _bundles.Add(assetBundleID, bundle);

                // TODO: Start a thread that invokes the callback when loading is complete
            }
        }

        public bool Unload(string assetBundle)
        {
            if (!_bundles.TryGetValue(assetBundle, out AssetBundle bundle)) return false;
            bundle.Unload(false);
            return true;
        }

        public void Dispose()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            _bundles.Clear();
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            BuildPipeline.BuildAssetBundles(UnityEngine.Application.streamingAssetsPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
#endif
    }
}
