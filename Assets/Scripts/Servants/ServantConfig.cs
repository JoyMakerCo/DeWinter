using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    [Serializable]
    public class ServantConfig : ScriptableObject, AmbitionEditor.ILocalizedAsset
    {
        public string Name;
        public string Title; // For Localization
        public string Description; // For Localization
        public ServantType Type;
        public ServantStatus Status;

        public int Wage;
        public SerializableHash<string, float>[] Modifiers = new SerializableHash<string, float>[0];

        public ServantVO GetServant()
        {
            ServantVO result = new ServantVO()
            {
                ID = name,
                Type = Type,
                Wage = Wage,
                Status = Status,
                Modifiers = new Dictionary<string, float>()
            };
            foreach (SerializableHash<string, float> mod in Modifiers)
            {
                result.Modifiers[mod.Key] = mod.Value;
            }
            return result;
        }

#if UNITY_EDITOR
        public Dictionary<string, string> Localize()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result[ServantConsts.SERVANT_LOC_KEY + name] = Name;
            result[ServantConsts.SERVANT_TITLE_KEY + name] = Title;
            result[ServantConsts.SERVANT_DESCRIPTION_KEY + name] = Description;
            return result;
        }

        [UnityEditor.MenuItem("Assets/Create/Create Servant")]
        public static void CreateServant()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<ServantConfig>("New Servant");
        }
#endif
    }
}
