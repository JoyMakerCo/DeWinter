using Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class AssetBundleSvc : IAppService, IInitializable, IDisposable
    {
        const string DIRECTORY_ID = "AssetBundles";
        private Dictionary<string, AssetBundle> _loaded;

        public void Initialize()
        {
            _loaded = new Dictionary<string, AssetBundle>();
        }

        public AssetBundle Load(string assetBundleID)
        {
            AssetBundle bundle;
            if (_loaded.TryGetValue(assetBundleID, out bundle)) return bundle;
            bundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, assetBundleID));
            if (bundle != null) _loaded[assetBundleID] = bundle;
            return bundle;
        }

        public bool Unload(string assetBundleID)
        {
            AssetBundle bundle;
            if (!_loaded.TryGetValue(assetBundleID, out bundle))
                return false;
            bundle.Unload(true);
            _loaded.Remove(assetBundleID);
            return true;
        }

        public bool IsLoaded(string assetBundleID) => _loaded.ContainsKey(assetBundleID);

        public void Dispose()
        {
            foreach(KeyValuePair<string, AssetBundle> bundle in _loaded)
            {
                bundle.Value.Unload(true);
            }
            _loaded.Clear();
            _loaded = null;
        }

#if (UNITY_EDITOR)
        //[UnityEditor.MenuItem("Assets/Build AssetBundles")]
        public static void AssetBundleBuilder()
        {
            string assetBundleDirectory = "Assets/" + DIRECTORY_ID;
            if (!UnityEngine.Windows.Directory.Exists(assetBundleDirectory))
            {
                UnityEngine.Windows.Directory.CreateDirectory(assetBundleDirectory);
            }
            UnityEditor.BuildPipeline.BuildAssetBundles(assetBundleDirectory, UnityEditor.BuildAssetBundleOptions.None, UnityEditor.BuildTarget.StandaloneOSX);
        }
#endif

    }
}
