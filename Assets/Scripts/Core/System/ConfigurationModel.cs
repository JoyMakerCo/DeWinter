using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Core
{
	public class ConfigurationModel
    {
        protected ConfigurationData _configData;

        const string _configName = "configuration";

        const string assetName = "ConfigurationData";
        const string assetExtension = ".asset";

        private static ConfigurationModel instance;

        public static ConfigurationModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigurationModel();
                }
                return instance;
            }
        }

		public ConfigurationModel()
		{
            if (_configData == null)
            {
                _configData = Resources.Load(_configName) as ConfigurationData;
                if (_configData == null)
                {
                    // If not found, autocreate the asset object.
                    _configData = ScriptableObject.CreateInstance<ConfigurationData>();
#if UNITY_EDITOR
                    var pathComponents = new string[] { UnityEngine.Application.dataPath, "Scripts", "Core", "System", "Resources" };
                    string properPath = Path.Combine(pathComponents);
                    if (!Directory.Exists(properPath))
                    {
                        Debug.LogError("Please to be creating Assets/Scripts/Core/System/Resources");
                    }
                    var fileComponents = new string[] { "Assets", "Scripts", "Core", "System", "Resources", assetName + assetExtension };

                    string fullPath = Path.Combine(fileComponents);
                    AssetDatabase.CreateAsset(_configData, fullPath);
#endif
                }
            }

            Debug.Log( "Configuration loaded: " + _configData.ToString() );
		}

        public static ConfigurationData Config
        {
            get
            {
                return Instance._configData;
            }
        }
    }
}