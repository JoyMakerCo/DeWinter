using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class CalendarButton : MonoBehaviour
    {
        private const string DECLINED = "declined";
        private const string ACCEPTED = "accepted";

        public Text dateText;
        public Image pastDayXImage;
        public Image liaisonIcon;
        public Image liaisonResponse;
        public GameObject RespondIndicator;

        public SpriteConfig CalendarSpriteConfig;
        public SpriteConfig LiaisonConfig;

        public Sprite TodaySprite;
        public Sprite WeekSprite;
        public Sprite MonthSprite;
        public Sprite OtherMonthSprite;

        //Party Indicators
        public Image Party1Icon;
        public Image Party1RSVPIcon;

        public Image Party2Icon;
        public Image Party2RSVPIcon;

        public Image CalendarBgImage;

        public DateTime Date { get; private set; }

        public void SetDay(DateTime day, DateTime today, int viewMonth)
        {
            Date = day;
            Party1Icon.gameObject.SetActive(false);
            Party1RSVPIcon.gameObject.SetActive(false);
            Party2Icon.gameObject.SetActive(false);
            Party2RSVPIcon.gameObject.SetActive(false);
            liaisonIcon.gameObject.SetActive(false);

            dateText.text = day.Day.ToString();

            CalendarBgImage.raycastTarget = (day >= today);
            pastDayXImage.gameObject.SetActive(day < today);

            if (day.Month != viewMonth) CalendarBgImage.sprite = OtherMonthSprite;
            else if (today.Month != viewMonth) CalendarBgImage.sprite = MonthSprite;
            else if (day.Day == today.Day) CalendarBgImage.sprite = TodaySprite;
            else if ((((int)(day.DayOfWeek + 1) % 7) - ((int)(today.DayOfWeek) + 1) % 7) == (day.Day - today.Day))
            { // Weekdays start on Monday for some reason, so shift the index by 1
                CalendarBgImage.sprite = WeekSprite;
            }
            else CalendarBgImage.sprite = MonthSprite;
        }

        public void SetEvents(PartyVO[] parties, RendezVO[] liaisons)
        {
            RendezVO rendez = liaisons.Length == 0
                ? null
                : Array.Find(liaisons, l => l.IsAttending)
                ?? Array.Find(liaisons, l => l.RSVP == RSVP.New)
                ?? liaisons[0];
            PartyVO party1 = parties.Length == 0
                ? null
                : Array.Find(parties, p => p.IsAttending)
                ?? Array.Find(parties, p => p.RSVP == RSVP.New)
                ?? parties[0];
            PartyVO party2 = rendez != null
                ? party1
                : Array.Find(parties, p => p != party1);
            bool showRespond = !Array.Exists(parties, p => p.IsAttending)
                && !Array.Exists(liaisons, r => r.IsAttending)
                && (Array.Exists(parties, p => p.RSVP == RSVP.New) || Array.Exists(liaisons, p => p.RSVP == RSVP.New && !p.IsCaller));
            if (rendez != null) party1 = null;

            RespondIndicator.gameObject.SetActive(showRespond);

            // Party2 Icon is centered
            Party2Icon.gameObject.SetActive(party1 != null);
            if (party1 != null)
            {
                Party2Icon.sprite = CalendarSpriteConfig.GetSprite(party1.Faction.ToString());
                Party2RSVPIcon.gameObject.SetActive(party1.RSVP != RSVP.New);
                Party2RSVPIcon.sprite = CalendarSpriteConfig.GetSprite(party1.IsAttending ? ACCEPTED : DECLINED);
            }

            // Party1 Icon is in corner
            Party1Icon.gameObject.SetActive(party2 != null);
            if (party2 != null)
            {
                Party1Icon.sprite = CalendarSpriteConfig.GetSprite(party2.Faction.ToString());
                Party1RSVPIcon.gameObject.SetActive(party2.RSVP != RSVP.New);
                Party1RSVPIcon.sprite = CalendarSpriteConfig.GetSprite(party2.IsAttending ? ACCEPTED : DECLINED);
            }

            // Liaison is centered
            liaisonIcon.gameObject.SetActive(rendez != null);
            if (rendez != null)
            {
                CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(rendez.Character);
                liaisonIcon.sprite = LiaisonConfig.GetSprite(character?.Faction.ToString());
                liaisonResponse.gameObject.SetActive(rendez.RSVP != RSVP.New);
                liaisonResponse.sprite = CalendarSpriteConfig.GetSprite(rendez.IsAttending ? ACCEPTED : DECLINED);
            }
        }

		public void OnClick() => AmbitionApp.SendMessage(CalendarMessages.SELECT_DATE, Date);
    }
}
