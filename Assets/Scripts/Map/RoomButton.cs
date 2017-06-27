using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Ambition;

namespace Ambition
{
	[Serializable]
	public class RoomStatusIndicator
	{
		public string ID;
		public Image Icon;
	}

	public class RoomButton : MonoBehaviour
	{
	    public Text DescriptionText;
	    public RoomStatusIndicator [] StatusIndicators;

	    private Outline _outline;
	    private Button _button;

		// Use this for initialization
		void Awake ()
		{
			_outline = this.gameObject.GetComponent<Outline>();
			_button = this.gameObject.GetComponent<Button>();
			_button.interactable = false;
			_outline.enabled = false;
			DescriptionText.enabled = false;

			foreach (RoomStatusIndicator indicator in StatusIndicators)
			{
				indicator.Icon.enabled = false;
			}
		}

		private RoomVO _room;
		public RoomVO Room
		{
			get { return _room; }
			set 
			{
				_room = value;
				if (_room != null)
				{
					DescriptionText.text = Room.Name + "\n" + new string('*', _room.Difficulty);
				}
			}
		}

		public void OnClick()
		{
			AmbitionApp.SendMessage<RoomVO>(MapMessage.GO_TO_ROOM, _room);
		}

		public void SetCurrentRoom(RoomVO currentRoom)
		{
			
			bool enable =
				_room != null &&
				currentRoom != null &&
				(_room == currentRoom || _room.IsNeighbor(currentRoom));

			_outline.enabled = enable;
			_button.interactable = enable;

			if (enable)
			{
				_outline.effectColor = (_room == currentRoom) ? Color.white : Color.black;
				if (!DescriptionText.enabled)
				{
					DescriptionText.enabled = true;
					foreach (RoomStatusIndicator indicator in StatusIndicators)
					{
						indicator.Icon.enabled = (Array.IndexOf(Room.Features, indicator.ID) >= 0);
					}
				}
			}
		}
	}
}