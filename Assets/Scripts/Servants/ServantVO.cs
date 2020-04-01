using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

using Core;

namespace Ambition
{
	public class ServantVO:  IConsoleEntity
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

		public override string ToString()
		{
			return string.Format( "{0} type: {1} wage £{2} status {3}", NameAndTitle, Type, Wage, Status );
		}
                
                
        public string[] Dump()
        {
			return new string[]
			{
				"id: " + ID,
				"name: " + NameAndTitle,
				"description: " + Description,
				"wage: " + Wage.ToString("£" + "#,##0") + "/week",
				"status: " + Status
			};
        }

        public void Invoke( string[] args )
        {
            ConsoleModel.warn("ServantVO has no invocation.");
        }  

	}
}
