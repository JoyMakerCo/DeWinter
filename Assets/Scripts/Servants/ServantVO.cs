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

		public bool Hired
		{
			get { return Tags.Contains(ServantConsts.HIRED); }
			set {
				if (!value) Tags.Remove(ServantConsts.HIRED);
				else if (!Tags.Contains(ServantConsts.HIRED))
					Tags.Add(ServantConsts.HIRED);
			}
		}

		public bool Introduced
		{
			get { return Tags.Contains(ServantConsts.INTRODUCED); }
			set {
				if (!value) Tags.Remove(ServantConsts.INTRODUCED);
				else if (!Tags.Contains(ServantConsts.INTRODUCED))
					Tags.Add(ServantConsts.INTRODUCED);
			}
		}

		[JsonProperty("tags")]
		public List<string> Tags = new List<string>();

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
