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
		public GuestActionVO Action;

		public int Opinion;

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
			State = guest.State;
		}

		public GuestState State = GuestState.Interested;
		
		public bool IsLockedIn
		{
            get { return State == GuestState.Offended || State == GuestState.Charmed; }
		}
	}
}
