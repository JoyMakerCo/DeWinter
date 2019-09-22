using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Ambition
{
	public class CalendarButton : MonoBehaviour
	{

	    public Image currentDayOutline;
	    public Text dateText;
	    public Image pastDayXImage;

		public SpriteConfig CalendarSpriteConfig;

	    //Party Indicators
	    public Image Party1Icon;
	    public Image NewParty1Icon;
	    public Image Party1RSVPIcon;

		public Image Party2Icon;
	    public Image NewParty2Icon;
	    public Image Party2RSVPIcon;

	    private Image _image;
	    private Color _defaultColor;
		private Button _btn;

        public DateTime Date { get; private set; }

        void Awake()
        {
            _image = GetComponent<Image>();
            _defaultColor = _image.color;
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(HandleClick);
            Party2RSVPIcon.sprite = CalendarSpriteConfig.GetSprite("declined");
            Party1RSVPIcon.enabled = false;
            Party2RSVPIcon.enabled = false;
        }

        public void SetDay(DateTime day, DateTime today, int viewMonth)
		{
            Date = day;
			Party1Icon.enabled = false;
			NewParty1Icon.enabled = false;
			Party1RSVPIcon.enabled = false;
			Party2Icon.enabled = false;
			NewParty2Icon.enabled = false;
			Party2RSVPIcon.enabled = false;

			_image.color = (day.Month == viewMonth)
				? _defaultColor
				: Color.Lerp(_defaultColor, Color.black, .5f);
			dateText.text = day.Day.ToString();

			currentDayOutline.enabled = (day == today);
            // By setting this at the last sibling in the holder parent object, the 'current day' frame doesn't get blocked by the other days around it.
            if (day == today) transform.SetAsLastSibling();

			_btn.interactable = (day >= today);
			pastDayXImage.enabled = day < today;
		}

		public void SetParties(PartyVO[] parties)
		{
            PartyVO party1 = null;
            PartyVO party2 = null;
            if (parties != null && parties.Length > 0)
            {
                party1 = Array.Find(parties, p => p.Attending || p.RSVP != RSVP.Declined) ?? parties[0];
                party2 = Array.Find(parties, p => p != party1 && !p.Attending);
            }

            Party2Icon.enabled = party1 != null;
            Party2Icon.sprite = CalendarSpriteConfig.GetSprite(party1?.Faction.ToString());
            Party2RSVPIcon.enabled = party1?.Attending ?? false;
            Party2RSVPIcon.sprite = CalendarSpriteConfig.GetSprite(party1?.RSVP == RSVP.Declined ? "declined" : "accepted");

            Party1Icon.enabled = party2 != null;
            Party1Icon.sprite = CalendarSpriteConfig.GetSprite(party2?.Faction.ToString());
            Party1RSVPIcon.enabled = party2?.RSVP == RSVP.Declined;
		}

		private void HandleClick() => AmbitionApp.SendMessage(CalendarMessages.SELECT_DATE, Date);
	}
}
