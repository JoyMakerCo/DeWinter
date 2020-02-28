﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class CharacterConfig : ScriptableObject, AmbitionEditor.ILocalizedAsset
    {
        public string Title;
        public string Background;
        public FactionType Faction;
        public AvatarVO Avatar;

        [Range(1, 100)]
        public int Favor;

        public CharacterConfig Spouse;
        public bool Celibate;

<<<<<<< Updated upstream
        CharacterVO GetCharacter() => new CharacterVO()
=======
        [SerializeField] private string _localizationKey;

        public string GetLocalizationKey() => _localizationKey;

        public CharacterVO GetCharacter() => new CharacterVO()
>>>>>>> Stashed changes
        {
            Name = name,
            Title = Title,
            Avatar = this.Avatar,
            Favor = Favor,
            Spouse = Spouse?.name, //TODO: Connect these characters in the model
            Faction = Faction,
            Background = Background,
            Celibate = Celibate,
        };
<<<<<<< Updated upstream
=======

        private string _FullName => string.IsNullOrWhiteSpace(FullName) ? this.DisplayName : FullName;
        private string _FormalName => string.IsNullOrWhiteSpace(FormalName) ? _FullName : FormalName;

#if UNITY_EDITOR
        public void SetLocalizationKey(string value)
        {
            UnityEditor.SerializedObject obj = new UnityEditor.SerializedObject(this);
            obj.FindProperty("_localizationKey").stringValue = value;
            obj.ApplyModifiedProperties();
        }

        public Dictionary<string, string> Localize()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            return result;
        }

        [UnityEditor.MenuItem("Ambition/Create/Character")]
        public static void CreateIncident()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<CharacterConfig>("New Character");
        }
#endif
>>>>>>> Stashed changes
    }
}
