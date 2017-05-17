using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class GuestViewMediator : MonoBehaviour
	{
		private const float DEFAULT_TIMER = 2.0f;

		private GuestVO _guest;
		private Image _guestImage;
		private GuestSprite _sprite;
		private GameObject _guestAvatar;
		private RemarkVO _remark;
		private bool _isIntoxicated=false;
		private IEnumerator _timer;
		private bool _isTargeted;
		private PartyArtLibrary _library; 
		public int index;

		public GameObject ArtLibrary;

		public Text GuestNameText;
		public Text guestInterestText;

		public Scrollbar guestInterestBar;
		public Image guestInterestBarImage;

		public Text guestOpinionText;
		public Scrollbar guestOpinionBar;
		public Image guestOpinionBarImage;
		public Image guestDispositionIcon;
		public PartyArtLibrary ArtConfig;

		void Awake()
		{
			_guestImage = GetComponent<Image>();
			_library = ArtLibrary.GetComponent<PartyArtLibrary>();
			AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Subscribe<float>(PartyMessages.START_TIMERS, HandleStartTimers);
			AmbitionApp.Subscribe<GuestVO []>(PartyMessages.GUESTS_TARGETED, HandleTargeted);
			AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
			AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Unsubscribe<float>(PartyMessages.START_TIMERS, HandleStartTimers);
			AmbitionApp.Unsubscribe<GuestVO []>(PartyMessages.GUESTS_TARGETED, HandleTargeted);
			AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
			AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);
		}

		private void HandleStartTimers(float seconds)
		{
			if (_timer != null)
				StopCoroutine(_timer);
			_timer = GuestTimer(seconds);
			StartCoroutine(_timer);
		}

		public IEnumerator GuestTimer(float seconds)
	    {
			for (float t = seconds; t >=0; t-=Time.deltaTime)
			{
			// TODO: Update the appearance of the timer
				yield return null;
			}
			_timer = null;
	    }

	    void OnMouseOver()
	    {
	    	if (_remark != null)
	    	{
	    		AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_TARGETED, _guest);
	    	}
	    }

	    void OnMouseDown()
	    {
	    	if (_remark != null)
	    	{
				AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_SELECTED, _guest);
	    	}
	    }

		// Don't handle the logic for updating guests in the view; let a command handle that
		private void HandleSelected(GuestVO guest)
		{
			_remark = null;
			_isTargeted = false;
			UpdateColor();
		}

		private void HandleRemark(RemarkVO remark)
		{
			_remark = remark;
			_isTargeted = false;
			UpdateColor();
		}

		private void HandleTargeted(GuestVO[] guests)
		{
			_isTargeted = (guests != null) && (Array.IndexOf(guests, _guest) >= 0);
			UpdateColor();
		}

		private void HandleGuests(GuestVO [] guests)
		{
			_guest = guests.Length > index ? guests[index] : null;
			this.gameObject.SetActive(_guest != null);
			if (_guest != null && _sprite != null)
			{
				bool isEnemy = (_guest is EnemyVO);

				GuestNameText.text = _guest.Name;
				GuestNameText.color = isEnemy ? Color.red : Color.white;
				guestInterestBar.image.color = isEnemy ? Color.red : Color.white;

				_guestImage.sprite = _sprite.GetSprite(_isIntoxicated ? 50 : _guest.Opinion);

				bool showInterestBar = (!isEnemy && _guest.State == GuestState.Ambivalent);
				guestInterestBar.gameObject.SetActive(showInterestBar);
            	guestInterestBarImage.gameObject.SetActive(showInterestBar);

            	UpdateColor();
			}
		}

		private void HandleIntoxication(int tox)
		{
			_isIntoxicated = (tox >= 50);
			
		}

		private void ResetTimer()
		{
			// Refill Interest of the Selected
			//Everyone loses one because of the Turn Timer
			HandleStartTimers(DEFAULT_TIMER + 1.0f);
		}

		private void UpdateColor()
		{
			Color c;
			if (_remark == null) c = Color.white; 
			else if (!_isTargeted) c = Color.gray;
			else if (_isIntoxicated) c = Color.white;
			else if (_remark.Topic == _guest.Like) c = Color.green;
			else if (_remark.Topic == _guest.Disike) c = Color.red;
			else c = Color.white;

			guestDispositionIcon.color = _guestImage.color = c;
		}

		// TODO: Good candidate for Localization
		string InterestState(GuestVO guest)
	    {
	    	EnemyVO enemy = guest as EnemyVO;
	        if (enemy == null)
	        {
	        	switch (guest.State)
	        	{
					case GuestState.Charmed:
						return "Charmed";
					case GuestState.PutOff:
						return "Put Off";
				}
				if (_timer == null)
					return "BORED!";
				return "Interested";
			}
			if (_timer != null)
			{
				switch (enemy.attackNumber)
				{
					case 0:
	                    return "Monopolize the Conversation";
	                case 1:
	                    return "Rumor Monger";
	                case 2:
	                    return "Belittle";
	                case 3:
	                    return "Antagonize";
				}
			}
			if (enemy.attackNumber < 0)
			{
				switch (enemy.State)
				{
					case GuestState.Charmed:
						return "Dazed";
					case GuestState.PutOff:
						return "Offended";
				}
				return "Plotting";
			}
			return "Attacking!";
	    }
	}
}