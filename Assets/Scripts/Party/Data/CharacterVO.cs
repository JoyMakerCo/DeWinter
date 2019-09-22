using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class CharacterVO
	{
        private string _name = null;
        public string Name
        {
            get => _name ?? (FirstName + " " + LastName);
            set => _name = value;
        }

        [JsonProperty("first_name")]
        public string FirstName;

        [JsonProperty("last_name")]
        public string LastName;

        [JsonProperty("faction")]
        public FactionType Faction = FactionType.Neutral;

		[JsonProperty("Gender")]
	    public Gender Gender;

        public int Favor;

        private string _title = null;
        public string Title
        {
            set => _title = value;
            get => !string.IsNullOrWhiteSpace(_title)
            ? _title
            : (Gender == Gender.Male
            ? "Monsieur"
            : Spouse != null
            ? "Madame"
            : "Mademoiselle");
        }

        public string ShortTitle => !string.IsNullOrWhiteSpace(_title)
            ? ""
            : Gender == Gender.Male
            ? "M"
            : Spouse != null
            ? "Mme"
            : "Mlle";


        public AvatarVO Avatar;

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
			_title = character._title;
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
            this.Avatar = avatar;
        }
    }
}
