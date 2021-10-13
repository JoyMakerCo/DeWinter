using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    public class CharacterConfig : ScriptableObject, AmbitionEditor.ILocalizedAsset
    {
        public string Name;
        public string FullName;
        public string FormalName;
        public string ShortName;
        public string Description;
        public string[] FavoredLocations;
        public string[] OpposedLocations;
        public FactionType Faction;
        public Gender Gender;
        public AvatarConfig Avatar;
        public bool Formal = false;
        public bool Acquainted = false;

        [Range(0, 100)]
        public int Favor;

        public CharacterVO GetCharacter() => new CharacterVO()
        {
            ID = this.name,
            Favor = Favor,
            Faction = Faction,
            Avatar = Avatar?.name,
            Gender = Gender,
            Acquainted = Acquainted,
            FavoredLocations = FavoredLocations,
            OpposedLocations = OpposedLocations,
            Formal = Formal
        };

        public Dictionary<string, string> Localize()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result[CharacterConsts.LOC_NAME + name] = Name;
            result[CharacterConsts.LOC_SHORT_NAME + name] = string.IsNullOrEmpty(ShortName) ? Name : ShortName;
            result[CharacterConsts.LOC_FULL_NAME + name] = string.IsNullOrEmpty(FullName) ? Name : FullName;
            result[CharacterConsts.LOC_FORMAL_NAME + name] = string.IsNullOrEmpty(FormalName) ? Name : FormalName;
            result[CharacterConsts.LOC_DESCRIPTION + name] = Description;
            return result;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/Character")]
        public static void CreateCharacter()
        {
            Util.ScriptableObjectUtil.CreateScriptableObject<CharacterConfig>("New Character");
        }
#endif
    }
}
