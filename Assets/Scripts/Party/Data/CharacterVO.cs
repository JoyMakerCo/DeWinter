using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.UI;

using Core;

namespace Ambition
{
    public class CharacterVO
    {
        [JsonProperty("id")]
        public string ID;

        [JsonProperty("faction")]
        public FactionType Faction = FactionType.None;

		[JsonProperty("Gender")]
	    public Gender Gender;

        [JsonProperty("favor")]
        public int Favor;

        [JsonProperty("dateable")]
        private bool _dateable;
        public bool IsDateable
        {
            get => Acquainted && _dateable;
            set => _dateable = value;
        }

        [JsonIgnore]
        public bool IsRendezvousScheduled => LiaisonDay >= 0;

        [JsonProperty("avatar")]
        public string Avatar;

        [JsonProperty("liaisonDay")]
        public int LiaisonDay=-1; // -1 = no liaison scheduled; 0+ = # of days from StartDate

        [JsonProperty("acquainted")]
        public bool Acquainted = false; // Whether the character is acquainted with the player

        [JsonProperty("locationsFavored")]
        public string[] FavoredLocations = new string[0];

        [JsonProperty("locationsOpposed")]
        public string[] OpposedLocations = new string[0];

        [JsonProperty("formal")]
        public bool Formal=false;

        [JsonProperty("rejections")]
        public int Rejections = 0;

        public CharacterVO() {}
		public CharacterVO(CharacterVO character)
		{
            ID = character.ID;
            Gender = character.Gender;
            Favor = character.Favor;
            Avatar = character.Avatar;
            Formal = character.Formal;
            FavoredLocations = CopyLocations(character.FavoredLocations);
            OpposedLocations = CopyLocations(character.OpposedLocations);
        }

        public override string ToString()
        {
			return "CharacterVO: " + ID +
                "\n Gender: " + Gender.ToString() +
                "\n Favor: " + Favor.ToString();
        }

        private string[] CopyLocations(string[] array)
        {
            int length = array?.Length ?? 0;
            string[] result = new string[length];
            if (array != null)
            {
                Array.Copy(array, result, length);
            }
            return result;
        }
    }
}
