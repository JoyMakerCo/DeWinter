using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using Core;

namespace Ambition
{
    public class LocationConfig : ScriptableObject, AmbitionEditor.ILocalizedAsset
    {
        public string Name;
        public string LocationWindowDescription;

        public IncidentConfig IntroIncidentConfig;
        public IncidentConfig[] StoryIncidentConfigs;
        public string SceneID; // Attach scene directly to this location and add to scene manager?
        public bool OneShot;
        public bool IsRendezvous;
        public bool IsDiscoverable;
        public Sprite LocationModalSprite;
        public RequirementVO[] Requirements;

#if UNITY_EDITOR
        public Dictionary<string, string> Localize()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result[ParisConsts.LABEL + name] = Name;
            result[ParisConsts.DESCRIPTION + name] = LocationWindowDescription;
            return result;
        }

        [MenuItem("Assets/Create/Paris Location")]
        public static void CreateLocConfig()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<LocationConfig>("New Location");
        }
#endif
    }
}
