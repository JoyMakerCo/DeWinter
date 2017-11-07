#if (UNITY_EDITOR)
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class LocalizedTextEditor : EditorWindow
{
	private Dictionary<string,Dictionary<string,string>> _phrases = new Dictionary<string, Dictionary<string, string>>();
	private static string _current = "Default";

    [MenuItem ("Window/Localization Editor")]
    static void Init()
    {
        EditorWindow.GetWindow (typeof(LocalizedTextEditor)).Show();
    }

    private void OnShow()
    {
		TextAsset file = Resources.Load<TextAsset>("Localization/" + _current);
		_phrases[_current] = new Dictionary<string, string>();
		JsonConvert.PopulateObject(file.text, _phrases[_current]);
    }

    private void OnSave()
    {
    	string jsonFile;
    	StreamWriter writer;
		foreach(KeyValuePair<string,Dictionary<string,string>> file in _phrases)
    	{
			writer = new StreamWriter("Localization/" + file.Key + ".json", false);
    		jsonFile = JsonConvert.SerializeObject(_phrases);
    		writer.Write(jsonFile);
    	}
    }
}
#endif
