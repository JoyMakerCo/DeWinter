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
			get {
				return (Opinion <= 0)
					? GuestState.PutOff
					: (Opinion >= 100)
					? GuestState.Charmed
					: GuestState.Ambivalent;
			}
		}
	}
}