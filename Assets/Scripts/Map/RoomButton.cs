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
		public Text StarIndicatorText;
	    public Outline OutLine;
	    public RoomStatusIndicator [] StatusIndicators;
	
		private bool _revealed;

		// Use this for initialization
		void Start ()
		{
			OutLine.enabled = _revealed = false;
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
				_room = Room;
				DescriptionText.text = Room.Name;
				StarIndicatorText.text = string.Concat(System.Linq.Enumerable.Repeat("*", Room.Difficulty));
			}
		}

		private bool _isAdjacent;
		public bool IsAdjacent
		{
			get { return _isAdjacent; }
			set
			{
				_isAdjacent = value;
				OutLine.enabled = value;
				if (!_revealed && value)
				{
					foreach (RoomStatusIndicator indicator in StatusIndicators)
					{
						indicator.Icon.enabled = (Array.IndexOf(Room.Features) >= 0);
					}
				}
				_revealed = true;
			}
	    }
	}
}