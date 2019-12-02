using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ambition
{

    public class ItemTemplates : ScriptableObject
    {
		public ItemConfig[] Items;


#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/ItemTemplates")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<ItemTemplates>("Item Templates");
#endif
    }
}
