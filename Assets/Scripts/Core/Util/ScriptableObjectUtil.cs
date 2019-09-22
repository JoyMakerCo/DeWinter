#if (UNITY_EDITOR)
using System;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Util
{
	public class ScriptableObjectUtil
	{
		public static T CreateScriptableObject<T>(string name=null) where T : ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T>();
			string path = (Selection.activeObject == null) ? "Assets" : AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
    		if( Path.GetExtension( path ) != "" )
				path = path.Substring(0, path.LastIndexOf('/'));
		    path = AssetDatabase.GenerateUniqueAssetPath (path + "/" + (name == null ? typeof(T).ToString() : name) + ".asset");

			AssetDatabase.CreateAsset (asset, path);
			AssetDatabase.SaveAssets ();
			Selection.activeObject = asset;
            return asset;
		}

        public static void CreatUniqueInstance<T>(string name=null) where T: ScriptableObject
        {
            string[] assets = AssetDatabase.FindAssets("t:" + typeof(T));
            if (assets.Length == 0) CreateScriptableObject<T>(name);            
        }
    }
}
#endif
