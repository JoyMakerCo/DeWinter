using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ambition
{
	public class ServantVO
	{
		[JsonProperty("id")]
		public string ID;

		[JsonProperty("name")]
		public string Name;

		[JsonProperty("type")]
		public string Type;

		[JsonProperty("description")]
		public string Description;

		[JsonProperty("slot")]
		public string Slot;

		[JsonProperty("wage")]
		public int Wage; //How much they cost per week

		[JsonProperty("state")]
		public Dictionary<string,object> State = new Dictionary<string, object>();

		[JsonProperty("status")]
		private string _status
		{
			set { Status = (ServantStatus)System.Enum.Parse(typeof(ServantStatus), value); }
		}

		public ServantStatus Status = ServantStatus.Unknown;

	    public string NameAndTitle
	    {
	    	get { return Name + ", the " + Slot; } // YOU ARE A SLOT!!
	    }

	    public string RecordData
	    {
			get { return Description + "\n- Costs " + Wage.ToString("£" + "#,##0") + "/Week"; }
	    }
	}
}
