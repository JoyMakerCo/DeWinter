using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class GuestViewMediator : MonoBehaviour
	{
		public int Position;

		private GuestVO _guest;
		private Image _guestImage;
		private bool _turnTimerActive;
		private GuestSprite _sprite;
		private GameObject _guestAvatar;

		public Text GuestNameText;
		public Image guestImage;
		public Text guestInterestText;

		public Scrollbar guestInterestBar;
		public Image guestInterestBarImage;

		public Text guestOpinionText;
		public Scrollbar guestOpinionBar;
		public Image guestOpinionBarImage;
		public Image guestDispositionIcon;

		private bool isEnemy
		{
			get { return _guest is Enemy; }
		}

		void Awake()
		{
			_guestImage = GetComponent<Image>();
			DeWinterApp.Subscribe<GuestVO []>(HandleGuests);
			DeWinterApp.Subscribe<float>(PartyMessages.START_TIMERS, HandleStartTimers);
			DeWinterApp.Subscribe<Remark>(PartyMessages.REMARK_SELECTED, HandleRemarkSelected);
		}

		void OnDestroy()
		{
			DeWinterApp.Unsubscribe<GuestVO []>(HandleGuests);
			DeWinterApp.Unsubscribe<float>(PartyMessages.START_TIMERS, HandleStartTimers);
			DeWinterApp.Unsubscribe<Remark>(PartyMessages.REMARK_SELECTED, HandleRemarkSelected);
		}

		public GuestSprite Avatar
		{
			set {
				_sprite = value;
				_guest = _guest;
			}
		}

		private void HandleGuests (GuestVO[] guests)
		{
			StopAllCoroutines();
			if (Position < guests.Length)
			{
				_guest = guests[Position];
				if (_guest != null && _sprite != null)
				{
					GuestNameText.text = _guest.Name;
					GuestNameText.color = isEnemy ? Color.red : Color.white;
					guestInterestBar.colors = isEnemy ? Color.red : Color.white;
	        		guestImage.sprite = GuestStateSprite(0);
	        		guestDispositionIcon.color = DispositionImageColor(0);
					_guestImage.sprite = _sprite.GetSpite(_guest.Opinion);

					bool showInterestBar = (!isEnemy && _guest.LockedInState != LockedInState.Interested);
					guestInterestBar.gameObject.SetActive(showInterestBar);
	            	guestInterestBarImage.gameObject.SetActive(showInterestBar);
				}
			}
			else
			{
				// Hide shit
			}
		}

		private void HandleStartTimers(float seconds)
		{
			StartCoroutine(GuestTimer(2.0f));
		}

		public IEnumerator GuestTimer(float seconds)
	    {
			for (float t = seconds; t >=0; t-=Time.deltaTime)
			{
				yield return null;
			}
	    }

		private void HandleRemarkSelected(Remark remark)
		{
			// Check to see if the remark applies to this guest
			// Change the appearance accordingly
		}
	}
}