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
				string partyName = AmbitionApp.GetString("party.name." + _party.ID);
				PartyNameTxt.text = partyName
                    ?? AmbitionApp.GetString("party.name.default", new Dictionary<string, string>(){
                        {"$HOST",_party.Host},
                        {"$IMPORTANCE",AmbitionApp.GetString("party_importance." + ((int)_party.Importance).ToString())},
                        {"$REASON",_party.Description}});
				FactionIcon.sprite = FactionSpriteConfig.GetSprite(_party.Faction);
                Highlight.enabled = _party.RSVP == RSVP.Accepted;
                Strikethrough.enabled = _party.RSVP == RSVP.Declined;
			}
		}

		void Awake()
		{
			_btn = GetComponent<Button>();
		}

		void OnEnable ()
		{
			_btn.onClick.AddListener(HandleClick);
		}

		void OnDisable ()
		{
			_btn.onClick.RemoveListener(HandleClick);
		}

		void HandleClick ()
		{
			AmbitionApp.SendMessage<System.DateTime>(CalendarMessages.SELECT_DATE, _party.Date);
		}
	}
}
