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
		private bool _isAdjacent;
		private bool _isCurrent;
	    private Button _button;

		// Use this for initialization
		void Awake()
		{
			_outline = this.gameObject.GetComponent<Outline>();
			_outline.enabled = false;

			_button = this.gameObject.GetComponent<Button>();
			_button.interactable = false;
			_button.onClick.AddListener(OnClick);

			DescriptionText.enabled = false;

			foreach (RoomStatusIndicator indicator in StatusIndicators)
			{
				indicator.Icon.enabled = false;
			}
		}

		void OnDestroy()
		{
			_button.onClick.RemoveListener(OnClick);
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

		public bool IsAdjacent
		{
			get { return _isAdjacent; }
			set {
				_isAdjacent = value;
				if (value) _isCurrent=false;
				UpdateDisplay();
			}
		}

		public bool IsCurrent
		{
			get { return _isCurrent; }
			set {
				_isCurrent = value;
				if (value) _isAdjacent=false;
				UpdateDisplay();
			}
		}

		protected void UpdateDisplay()
		{
			bool reveal=false;
			if (_isAdjacent)
			{
				_outline.effectColor = Color.black;
				reveal = true;
			}
			else if (_isCurrent)
			{
				_outline.effectColor = Color.white;
				reveal = true;
			}
			_outline.enabled = reveal;
			_room.Revealed = reveal || _room.Revealed;
			_button.interactable = _isAdjacent;

			if (!DescriptionText.enabled && reveal)
			{
				DescriptionText.enabled = true;
				foreach (RoomStatusIndicator indicator in StatusIndicators)
				{
					indicator.Icon.enabled = (Array.IndexOf(Room.Features, indicator.ID) >= 0);
				}
			}
		}

		protected void OnClick()
		{
			AmbitionApp.SendMessage<RoomVO>(MapMessage.GO_TO_ROOM, _room);
		}
	}
}
