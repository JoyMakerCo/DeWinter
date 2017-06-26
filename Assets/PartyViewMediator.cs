using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class PartyViewMediator : MonoBehaviour
	{
		public GameObject MapView;
		public GameObject RoomView;

		// Use this for initialization
		void Awake()
		{
			DeWinterApp.Subscribe<RoomVO>(GoToRoom);
		}

		void OnDestroy()
		{
			DeWinterApp.Subscribe<RoomVO>(GoToRoom);
		}

		void Start ()
		{
			MapView.SetActive(true);
			RoomView.SetActive(false);
		}

		private void GoToRoom(RoomVO room)
		{
			MapView.SetActive(room.Cleared);
			RoomView.SetActive(!room.Cleared);
		}
	}
}
