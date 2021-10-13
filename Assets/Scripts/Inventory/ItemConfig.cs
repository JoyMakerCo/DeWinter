using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ambition
{
    // The name of the item config acts as the item's unique ID
    public class ItemConfig : ScriptableObject, AmbitionEditor.ILocalizedAsset
    {
        public string Name;
        public string Description;

        [Serializable]
        public struct ItemState
        {
            public string Key;
            public string Value;
        };

        public Sprite Asset;
        public ItemType Type;
        public int Price;
        public ItemState[] State;
        public bool Permanent = false;
        public bool Market = true;

#if UNITY_EDITOR
        public Dictionary<string, string> Localize()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result[name + ItemConsts.ITEM_LOC_NAME] = Name;
            result[name + ItemConsts.ITEM_LOC_DESCRIPTION] = Description;
            return result;
        }

        [MenuItem("Assets/Create/Item")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<ItemConfig>("New Item");
    }

    [CustomPropertyDrawer(typeof(ItemConfig.ItemState))]
    public class ItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Don't make child fields be indented
            // Calculate rects
            Rect keyRect = new Rect(position.x, position.y, position.width*.5f, position.height);
            Rect valueRect = new Rect(position.width * .5f, position.y, position.width * .5f, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("Key"), GUIContent.none);
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Value"), GUIContent.none);

            EditorGUI.EndProperty();
        }
#endif
    }
}
