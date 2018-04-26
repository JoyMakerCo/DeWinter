using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class GuestCountMediator : MonoBehaviour
	{
		public int Count;

		void Awake()
		{
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
		}

		void OnEnable()
		{
			MapModel map = AmbitionApp.GetModel<MapModel>();
			if (map.Room != null) HandleGuests(map.Room.Guests);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);
		}

		private void HandleGuests(GuestVO [] guests)
		{
			this.gameObject.SetActive(guests != null && guests.Length == Count);
		}
	}
}
