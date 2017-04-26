using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class GuestViewMediator : MonoBehaviour
	{
		private GuestVO _guest;
		private Image _guestImage;
		private bool _turnTimerActive;
		private GuestSprite _sprite;
		private GameObject _guestAvatar;
		private bool _targeted;
		private RemarkVO _remark;

		public Text GuestNameText;
		public Text guestInterestText;

		public Scrollbar guestInterestBar;
		public Image guestInterestBarImage;

		public Text guestOpinionText;
		public Scrollbar guestOpinionBar;
		public Image guestOpinionBarImage;
		public Image guestDispositionIcon;

		public GuestVO Guest
		{
			get { return _guest; }
			set {
				_guest = Guest;
				this.gameObject.SetActive(_guest != null);
				UpdateSprite();
			}
		}

		public GuestSprite Avatar
		{
			set {
				_sprite = value;
				UpdateSprite();
			}
		}

		void Awake()
		{
			_guestImage = GetComponent<Image>();
			DeWinterApp.Subscribe<RemarkVO>(HandleRemark);
			DeWinterApp.Subscribe<float>(PartyMessages.START_TIMERS, HandleStartTimers);
			DeWinterApp.Subscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleTargeted);
		}

		void OnDestroy()
		{
			DeWinterApp.Unsubscribe<RemarkVO>(HandleRemark);
			DeWinterApp.Unsubscribe<float>(PartyMessages.START_TIMERS, HandleStartTimers);
			DeWinterApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleTargeted);
		}

		private void HandleStartTimers(float seconds)
		{
			StartCoroutine(GuestTimer(2.0f));
		}

		public IEnumerator GuestTimer(float seconds)
	    {
			for (float t = seconds; t >=0; t-=Time.deltaTime)
			{
			// TODO: Update the appearance of the timer
				yield return null;
			}
	    }

		private void HandleRemark(RemarkVO remark)
		{
			_remark = remark;
			if (_targeted) HandleTargeted(_guest);
		}

		private void HandleTargeted(GuestVO guest)
		{
			_targeted = (guest != null) && (_targeted || guest == _guest);
			_guestImage.color = GetColor();
		}

		private void GetColor()
		{
			if (!_targeted) return Color.white;
			if (_remark != null) return Color.grey;
			// TODO: Figure out dispositions
/*
			if(!room.Guests[guestNumber].dispositionRevealed && party.currentPlayerIntoxication >= 50)
	        {
	            return Color.gray;
	        }
	        if (party.playerHand[targetingRemark].tone == room.Guests[guestNumber].disposition.like) //They like the tone
	        {
	            return Color.green;
	        }
	        else if (party.playerHand[targetingRemark].tone == room.Guests[guestNumber].disposition.dislike) //They dislike the tone
*/
			return Color.grey;
		}

		private void UpdateSprite()
		{
			if (_guest != null && _sprite != null)
			{
				bool isEnemy = (_guest is Enemy);

				GuestNameText.text = _guest.Name;
				GuestNameText.color = isEnemy ? Color.red : Color.white;
				guestInterestBar.colors = isEnemy ? Color.red : Color.white;
        		guestDispositionIcon.color = GetColor();
				_guestImage.sprite = _sprite.GetSpite(_guest.Opinion);

				bool showInterestBar = (!isEnemy && _guest.LockedInState != LockedInState.Interested);
				guestInterestBar.gameObject.SetActive(showInterestBar);
            	guestInterestBarImage.gameObject.SetActive(showInterestBar);
			}
		}
	}
}