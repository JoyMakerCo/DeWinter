using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class HostViewMediator : MonoBehaviour
	{
		void Awake()
		{
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);
		}

		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<RoomVO>(HandleRoom);
		}
		
		private void HandleRoom(RoomVO room)
		{
			this.gameObject.SetActive(room.HostHere);
		}
	}
}