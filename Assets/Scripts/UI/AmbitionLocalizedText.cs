using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ambition
{
    // Component for localizing a Text UI element on the same gameobject
    // Localization will default to the specified key if one exists;
    // Otherwise, the stored generated key for the element will be used instead.
    public class AmbitionLocalizedText : MonoBehaviour
    {
        [Serializable]
        public struct LocalizedTextField
        {
            public Text TextField;
            public string LocalizationKey;
        }

        public LocalizedTextField[] LocalizedText;

        void Awake()
        {
            foreach (LocalizedTextField row in LocalizedText)
            {
                row.TextField.text = AmbitionApp.Localize(row.LocalizationKey);
            }
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(AmbitionLocalizedText.LocalizedTextField))]
    public class LocalizationRowDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float w = position.width * .3f;
            Rect RefRect = new Rect(position.x, position.y, w, position.height);
            Rect KeyRect = new Rect(w, position.y, position.width - w, position.height);

            SerializedProperty tf = property.FindPropertyRelative("TextField");

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.ObjectField(RefRect, tf, GUIContent.none);
            string key = EditorGUI.TextField(KeyRect, property.FindPropertyRelative("LocalizationKey").stringValue);
            property.FindPropertyRelative("LocalizationKey").stringValue = key;

            EditorGUI.EndProperty();
        }
    }
#endif  
}
