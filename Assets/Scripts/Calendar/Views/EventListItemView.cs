using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ambition
{
	public class EventListItemView : MonoBehaviour
	{
		public Image FactionIcon;
        public Image Checkmark;
        public Image Strikethrough;
		public Text PartyNameTxt;
        public Text DateText;
		public SpriteConfig FactionSpriteConfig;
        public Image TodayIndicator;

        private DateTime _date;

        private CalendarEvent _event;
        public CalendarEvent Event
        {
            get => _event;
            set
            {
                PartyVO party = value as PartyVO;
                RendezVO rendez = value as RendezVO;
                _date = AmbitionApp.Calendar.StartDate.AddDays(value.Day);
                _event = value;
                if (party != null)
                {
                    PartyNameTxt.text = AmbitionApp.GetModel<LocalizationModel>().GetPartyName(party);
                    FactionIcon.sprite = FactionSpriteConfig.GetSprite(party.Faction.ToString());
                }
                else if (rendez != null)
                {
                    CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(rendez.Character);
                    PartyNameTxt.text = AmbitionApp.GetModel<LocalizationModel>().GetShortName(character, rendez.Character);
                    FactionIcon.sprite = FactionSpriteConfig.GetSprite("seal." + character.Faction.ToString());
                }
                TodayIndicator.enabled = value.Day == AmbitionApp.Calendar.Day;
                Checkmark.enabled = _event.IsAttending;
                Strikethrough.enabled = _event.RSVP == RSVP.Declined;
                DateText.text = _date.Day + "/" + _date.Month;
            }
        }

        public void HandleClick() => AmbitionApp.SendMessage(CalendarMessages.SELECT_DATE, _date);
	}
}
