using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ambition
{
    // The name of the item config acts as the item's unique ID
    public class ItemConfig : ScriptableObject
    {
        [Serializable]
        public struct ItemState
        {
            public string Key;
            public string Value;
        };

        public ItemVO GetItem()
        {
            ItemVO item = new ItemVO()
            {
                Type = this.Type,
                Price = this.Price,
                State = new Dictionary<string, string>(),
                Asset = Asset
            };
            if (State != null)
            {
                Array.ForEach(State, s => item.State[s.Key] = s.Value);
            }
            return item;
        }

        public Sprite Asset;
        public ItemType Type;
        public int Price;
        public ItemState[] State;

#if (UNITY_EDITOR)
        [MenuItem("Assets/Create/Item")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<ItemConfig>("New Item");
#endif
    }

    [Serializable]
    public class InventoryItem
    {
        public ItemVO GetItem()
        {
            if (config == null) return null;
            ItemVO item = config.GetItem();
            if (State != null)
            {
                Array.ForEach(State, s => item.State[s.Key] = s.Value);
            }
            if (SellbackPrice >= 0) item.Price = SellbackPrice;
            item.Name = this.Name;
            item.Equipped = Equipped;
            return item;
        }

        public string Name;
        public ItemConfig config;
        public int SellbackPrice = -1;
        public bool Equipped = false;
        public ItemConfig.ItemState[] State;
    }

#if UNITY_EDITOR
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
    }
#endif
}
