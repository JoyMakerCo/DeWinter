using System;
using UnityEngine;

namespace Ambition
{
	public class GuestViewInputMediator : MonoBehaviour
	{
		public int Index;

		private GuestVO _guest;

		void Awake()
		{
			AmbitionApp.Subscribe<GuestVO []>(HandleGuests);
		}

		void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<GuestVO []>(HandleGuests);
	    }

	    private void HandleGuests(GuestVO[] guests)
	    {
	    	_guest = guests[Index];
	    }

		void OnMouseOver()
		{
			AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_TARGETED, _guest);
		}

		void OnMouseOut()
	    {
	    	AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_TARGETED, null);
	    }

		void OnMouseUp()
	    {
			AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_SELECTED, _guest);
	    }
	}
}
