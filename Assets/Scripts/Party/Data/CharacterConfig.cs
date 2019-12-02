using System;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    public class CharacterConfig : ScriptableObject
    {
        public string DisplayName;
        public string FullName;
        public string FormalName;
        public string Background;
        public FactionType Faction;
        public string AvatarID;

        [Range(1, 100)]
        public int Favor;

        public CharacterConfig Spouse;
        public bool Celibate;

        public CharacterVO GetCharacter() => new CharacterVO()
        {
            ID = this.name,
            Name = this.DisplayName,
            FullName = _FullName,
            FormalName = _FormalName,
            AvatarID = this.AvatarID,
            Favor = Favor,
            Spouse = Spouse?.name,
            Faction = Faction,
            Background = Background,
            Celibate = Celibate,
        };

        private string _FullName => string.IsNullOrWhiteSpace(FullName) ? this.DisplayName : FullName;
        private string _FormalName => string.IsNullOrWhiteSpace(FormalName) ? _FullName : FormalName;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/Character")]
        public static void CreateIncident()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<CharacterConfig>("New Character");
        }
#endif
    }
}
