using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class RoomTitleScript : MonoBehaviour
	{
		private Text _titleText;

		void Awake ()
		{
			_titleText = gameObject.GetComponent<Text>();
			AmbitionApp.Subscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
		}

		private void HandleRoom(RoomVO room)
		{
            _titleText.text = room.Name;
		}
	}
}
