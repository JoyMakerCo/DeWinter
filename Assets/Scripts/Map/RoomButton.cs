using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

namespace DeWinter
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
	    public Outline outLine;
	    public RoomStatusIndicator [] StatusIndicators;
	
		// Use this for initialization
		void Start ()
		{
			outLine.enabled = false;
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
					string txt = Room.Name + "\n" + new string('*', _room.Difficulty);
					DescriptionText.text = txt;
				}
			}
		}

		public void OnClick()
		{
			DeWinterApp.SendMessage<RoomVO>(MapMessage.GO_TO_ROOM, _room);
		}

		public void UpdateRoom(RoomVO room, bool isAdjacent, bool isCurrent)
		{
			_room = room;
			outLine.enabled = isAdjacent;
			outLine.effectColor = isCurrent ? Color.white : Color.black;
			if (!_room.Revealed && isAdjacent)
			{
				foreach (RoomStatusIndicator indicator in StatusIndicators)
				{
					indicator.Icon.enabled = (Array.IndexOf(Room.Features, indicator.ID) >= 0);
				}
			}
		}
	}
}