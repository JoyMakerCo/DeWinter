using System;
using UnityEngine;

namespace Ambition
{
	public class GuestViewsMediator : MonoBehaviour
	{
		public GuestViewMediator[] Guests;
		public PartyArtLibrary ArtLibrary;

		void Start ()
		{
			for (int i=Guests.Length-1; i>=0; i--)
			{
				Guests[i].Index = i;
				Guests[i].ArtLibrary = ArtLibrary;
			}
		}
	}
}

