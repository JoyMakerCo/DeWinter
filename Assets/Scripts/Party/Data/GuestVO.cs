using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
	public class GuestVO
	{
		[JsonProperty("Name")]
	    public string Name;

		[JsonProperty("Like")]
	    public string Like;

		[JsonProperty("Dislike")]
		public string Disike;

		[JsonProperty("IsFemale")]
	    public bool IsFemale;

		[JsonProperty("Variant")]
	    public int Variant=-1;

		[JsonProperty("Opinion")]
		protected int _opinion;

		[JsonIgnore]
		public int Opinion
		{
			get { return _opinion; }
			set
			{
				if (value >= 100)
				{
					_opinion = 100;
					State = GuestState.Charmed;
				}
				else if (value <= 0)
				{
					_opinion = 0;
					State = GuestState.PutOff;
				}
				else
				{
					_opinion = value;
					State = GuestState.Interested;
				}
			}
		}

		public GuestVO() {}
		public GuestVO(GuestVO guest)
		{
			Name = guest.Name;
			Like = guest.Like;
			Disike = guest.Disike;
			IsFemale = guest.IsFemale;
			Opinion = guest.Opinion;
		}

		public GuestState State;

		public bool IsLockedIn
		{
			get { return State == GuestState.PutOff || State == GuestState.Charmed; }
		}
	}
}