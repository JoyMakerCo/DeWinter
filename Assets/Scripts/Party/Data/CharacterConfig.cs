using System;
using UnityEngine;

namespace Ambition
{
    public class CharacterConfig : ScriptableObject
    {
        public string Title;
        public string Background;
        public FactionType Faction;
        public AvatarVO Avatar;

        [Range(1, 100)]
        public int Favor;

        public CharacterConfig Spouse;
        public bool Celibate;

        CharacterVO GetCharacter() => new CharacterVO()
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
    }
}
