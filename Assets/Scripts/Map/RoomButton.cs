using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using Ambition;
using Util;

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
		public ColorConfig ColorConfig;
		public SpriteConfig FloorTexturtes;

	    private Button _button;
	    private RoomGraphic _graphic;

		// Use this for initialization
		void Awake()
		{
			ColorBlock cb;
			_button = this.gameObject.GetComponent<Button>();
			_button.interactable = false;
			_button.onClick.AddListener(OnClick);
			cb = _button.colors;
			cb.highlightedColor = ColorConfig.GetColor("highlight");
			cb.normalColor = ColorConfig.GetColor("adjacent");
			cb.disabledColor = ColorConfig.GetColor("hidden");
			_button.colors = cb;
			_graphic = GetComponent<RoomGraphic>();
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
				_graphic.Room = value;
				gameObject.name = value.Name;

				if (_room == null) // This will crash if the room has no walls.
				{
					MapModel map = AmbitionApp.GetModel<MapModel>();
					float scale = map.MapScale;
					RectTransform xform = gameObject.GetComponent<RectTransform>();
					int[] bounds = value.GetBounds();
					xform.anchoredPosition = new Vector2(bounds[0]*scale, bounds[1]*scale);
					xform.sizeDelta = new Vector2((bounds[2]-bounds[0])*scale, (bounds[3]-bounds[1])*scale);

					_graphic.sprite = FloorTexturtes.Sprites[new System.Random().Next(FloorTexturtes.Sprites.Length)].Sprite;
				}
				_room = value;
			}
		}

		public void UpdatePlayerRoom(RoomVO room)
		{
			_button.interactable = _room.IsAdjacentTo(room);
			if (!DescriptionText.enabled && (_button.interactable || room == _room))
			{
				ColorBlock cb = _button.colors;
				cb.disabledColor = ColorConfig.GetColor("shown");
				_button.colors = cb;

				DescriptionText.enabled = true;
				DescriptionText.text = _room.Name + "\n" + new string('*', _room.Difficulty);
				foreach (RoomStatusIndicator indicator in StatusIndicators)
				{
					indicator.Icon.enabled = (Array.IndexOf(_room.Features, indicator.ID) >= 0);
				}
			}

			if (_room == room)
			{
				Color c = ColorConfig.GetColor("current");
				_button.GetComponent<CanvasRenderer>().SetColor(c);
			}
		}

		protected void OnClick()
		{
			AmbitionApp.SendMessage<RoomVO>(MapMessage.GO_TO_ROOM, _room);
		}
	}
}
