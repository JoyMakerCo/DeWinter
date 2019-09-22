using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class EventListItemView : MonoBehaviour
	{
		public Image FactionIcon;
		public Image Strikethrough;
		public Image Highlight;
		public Text PartyNameTxt;
		public SpriteConfig FactionSpriteConfig;

		private Button _btn;

		private PartyVO _party;
		public PartyVO Party
		{
			get { return _party; }
			set {
				_party = value;
                PartyNameTxt.text = _party.Name;
				FactionIcon.sprite = FactionSpriteConfig.GetSprite(_party.Faction.ToString());
                Highlight.enabled = _party.RSVP == RSVP.Accepted || _party.RSVP == RSVP.Required;
                Strikethrough.enabled = _party.RSVP == RSVP.Declined;
			}
		}

		void Awake() => _btn = GetComponent<Button>();
		void OnEnable () => _btn.onClick.AddListener(HandleClick);
		void OnDisable () => _btn.onClick.RemoveListener(HandleClick);
		void HandleClick () => AmbitionApp.SendMessage(CalendarMessages.SELECT_DATE, _party.Date);
	}
}
