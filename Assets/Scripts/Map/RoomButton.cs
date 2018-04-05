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
	public class RoomButton : MonoBehaviour
	{
	    public Text DescriptionText;
	    public Image Punchbowl;
	    public Image Host;
		public SpriteConfig FactionIcons;

		public ColorConfig ColorConfig;
		public SpriteConfig FloorTexturtes;

	    private Button _button;
	    private RoomGraphic _graphic;

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
					xform.anchoredPosition = new Vector2(value.Bounds[0]*scale, value.Bounds[1]*scale);
					xform.sizeDelta = new Vector2((value.Bounds[2]-value.Bounds[0])*scale, (value.Bounds[3]-value.Bounds[1])*scale);

					_graphic.sprite = FloorTexturtes.Sprites[Util.RNG.Generate(0, FloorTexturtes.Sprites.Length)].Sprite;
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
				DescriptionText.text = _room.Name + "\n";
				for (int i=_room.Difficulty-1; i>=0; i--)
					DescriptionText.text += '\u2605';
				Punchbowl.enabled = (Array.IndexOf(_room.Features, PartyConstants.PUNCHBOWL) >= 0);
				Host.enabled = (Array.IndexOf(_room.Features, PartyConstants.HOST) >= 0);
				if (Host.enabled)
				{
					PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
					Host.sprite = FactionIcons.GetSprite(party.Faction);
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
