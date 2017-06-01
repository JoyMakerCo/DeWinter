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

	    public int Interest;

		[JsonProperty("Variant")]
	    public int Variant=-1;

		[JsonProperty("Opinion")]
		public int Opinion;

		public GuestVO() {}
		public GuestVO(GuestVO guest)
		{
			Name = guest.Name;
			Like = guest.Like;
			Disike = guest.Disike;
			IsFemale = guest.IsFemale;
			Opinion = guest.Opinion;
		}

		public GuestState State
		{
			get
			{
				if (Interest <= 0) return GuestState.Bored;
				if (Opinion >= 100) return GuestState.Charmed;
				if (Opinion <= 0) return GuestState.PutOff;
				return GuestState.Interested;
			}
		}

		public bool IsLockedIn
		{
			get { return State == GuestState.PutOff || State == GuestState.Charmed; }
		}
	}
}