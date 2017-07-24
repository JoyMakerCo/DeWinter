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
			AmbitionApp.Subscribe(PartyMessages.SHOW_ROOM, GoToRoom);
			AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, GoToMap);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, GoToRoom);
			AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, GoToMap);
		}

		void Start ()
		{
			AmbitionApp.SendMessage(PartyMessages.SHOW_MAP);
		}

		private void GoToRoom()
		{
			MapView.SetActive(false);
			RoomView.SetActive(true);
		}

		private void GoToMap()
		{
			MapView.SetActive(true);
			RoomView.SetActive(false);
		}
	}
}
