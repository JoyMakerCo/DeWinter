using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.UI;

using Core;

namespace Ambition
{
    public class CharacterVO : IConsoleEntity
    {

        [JsonProperty("display_name")]
        public string Name;

        [JsonProperty("id")]
        public string ID;

        [JsonProperty("full_name")]
        public string FullName;

        [JsonProperty("formal_name")]
        public string FormalName;

        [JsonProperty("faction")]
        public FactionType Faction = FactionType.Neutral;

		[JsonProperty("Gender")]
	    public Gender Gender;

        public int Favor;

        public string AvatarID;

        [JsonProperty("Reputation")]
        public int Reputation;

        [JsonProperty("Background")]
        public string Background;

        [JsonProperty("Wealth")]
        public int Wealth;

        [JsonProperty("Spouse")]
        public string Spouse=null;

        [JsonProperty("Celibate")]
        public bool Celibate = false;

		public CharacterVO() {}
		public CharacterVO(CharacterVO character)
		{
			Name = character.Name;
            ID = character.ID;
            FullName = character.FullName;
            FormalName = character.FormalName;
            Gender = character.Gender;
            Spouse = character.Spouse;
            Reputation = character.Reputation;
            Background = character.Background;
            Wealth = character.Wealth;
            Celibate = character.Celibate;
        }

        public CharacterVO(string name, AvatarVO avatar)
        {
            Name = name;
            this.AvatarID = avatar.ID;
        }

        public override string ToString()
        {
            return string.Format("CharacterVO: {0}", Name );
        }

        public string[] Dump()
        {
			return new string[]
			{
				"CharacterVO: " + ID,
                "Display Name: " + Name,
                "FullName: " + FullName,
                "FormalName: " + FormalName,
                "Gender: " + Gender.ToString(),
				"Spouse: " + Spouse,
				"Reputation: " + Reputation,
				"Background: " + Background,
				"Favor: " + Favor.ToString(),
				"Wealth: " + Wealth.ToString(),
				"Celibate: " + (Celibate?"true":"false")	
			};
        }

        public void Invoke( string[] args )
        {
            ConsoleModel.warn("CharacterVO has no invocation.");
        }
    }
}
