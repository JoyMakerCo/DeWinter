using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

namespace Ambition
{
	public class ServantVO
	{
		[JsonProperty("name")]
		public string Name; //What is their given name?
		
		[JsonProperty("slot")]
	    public string slot; //What role do they fill? Seamstress? Spymaster? Bodyguard?

		[JsonProperty("title")]
		public string title; //Title, just used in name stuff

		[JsonProperty("description")]
		public string description; //What this servant can do for the Player

		[JsonProperty("wage")]
		public int Wage; //How much they cost per week

		public bool Hired=false;
		public bool Introduced=false;

	    public string NameAndTitle
	    {
	    	get { return Name + ", the " + title; }
	    }

	    public string RecordData
	    {
			get { return description + "\n- Costs " + Wage.ToString("£" + "#,##0") + "/Week"; }
	    }
	}
}