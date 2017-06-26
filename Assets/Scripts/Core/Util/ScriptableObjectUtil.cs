using System;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Util
{
	public class ScriptableObjectUtil
	{
		public static void CreateScriptableObject<T>() where T : ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T>();
			string path = (Selection.activeObject == null) ? "Assets" : AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
    		if( Path.GetExtension( path ) != "" )
				path = path.Substring(0, path.LastIndexOf('/'));
		    path = AssetDatabase.GenerateUniqueAssetPath (path + "/" + typeof(T).ToString() + ".asset");

			AssetDatabase.CreateAsset (asset, path);
			AssetDatabase.SaveAssets ();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = asset;
		}
	}
}
