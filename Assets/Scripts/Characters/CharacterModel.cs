using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Core;

namespace Ambition
{
	public class CharacterModel : DocumentModel, IConsoleEntity
	{
        [JsonProperty("SeductionMod")]
		public int SeductionModifier;

		[JsonProperty("SeductionAltMod")]
		public int SeductionAltModifier;

		[JsonProperty("SeductionMarriageMod")]
		public int SeductionMarriedModifier;

		[JsonProperty("SeductionTimeMod")]
		public int SeductionTimeModifier;

		[JsonProperty("SeductionDevotion")]
		public int SeductionDevotion;

        [JsonIgnore]
        public Dictionary<string, CharacterVO> Characters = new Dictionary<string, CharacterVO>();

        [JsonProperty("Characters")]
        private CharacterVO[] _characters
        {
            set => Array.ForEach(value, c => Characters.Add(c.Name, c));
        }

        public CharacterModel() : base("CharacterData") {}

		// For debug console, return characters by loose ID
		public CharacterVO[] GetCharacters( string id )
		{
			return ConsoleUtilities.Lookup(id,Characters).Select( k => Characters[k] ).ToArray();
		}

		public string[] Dump()
        {
            var lines = new List<string>()
            {
                "CharacterModel:",
				"Seduction Mod: "+SeductionModifier.ToString(),
				"Seduction Alt Mod: "+SeductionAltModifier.ToString(),
				"Seduction Married Mod: "+SeductionMarriedModifier.ToString(),
				"Seduction Time Mod: "+SeductionTimeModifier.ToString(),
				"Seduction Devotion: "+SeductionDevotion.ToString(),
				"Characters: "
			};
				
			foreach (var kv in Characters)
			{
				lines.Add( "  "+kv.Value.ToString());
			}
			return lines.ToArray();
        }

        public void Invoke( string[] args )
        {
            ConsoleModel.warn("CharacterModel has no invocation.");
        }	
	}
}
