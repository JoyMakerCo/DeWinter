using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
	public class GuestVO
	{
	    public string Name
	    {
	    	get {
	    		return Title + " " + DisplayName;
    		}
	    }

		[JsonProperty("title")]
	    public string Title;

		[JsonProperty("first_name")]
	    public string FirstName;

		[JsonProperty("last_name")]
	    public string LastName;

	    public string DisplayName
	    {
	    	get {
				return FirstName + " " + LastName;
			}
	    }

		[JsonProperty("Like")]
	    public string Like;

		[JsonProperty("Dislike")]
		public string Dislike;

		[JsonProperty("Gender")]
	    public Gender Gender;

		[JsonProperty("Avatar")]
	    public string Avatar;

		public int Opinion;
		public int MaxInterest;
		public int Interest;

		public GuestVO() {}
		public GuestVO(GuestVO guest)
		{
			FirstName = guest.FirstName;
			LastName = guest.LastName;
			Title = guest.Title;
			Like = guest.Like;
			Dislike = guest.Dislike;
			Gender = guest.Gender;
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
