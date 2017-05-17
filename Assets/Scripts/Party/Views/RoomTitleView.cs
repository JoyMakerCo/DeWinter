using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class RoomTitleView : MonoBehaviour
	{
		private Text _text;

		void Awake()
		{
			_text = gameObject.GetComponent<Text>();
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RoomVO>(HandleRoom);
		}

		private void HandleRoom(RoomVO room)
		{
			_text.text = room.Name;
		}
	}
}