using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public class GuestViewMediator : MonoBehaviour
	{
		public int index;

		public Image GuestImage;
		public Text GuestNameText;
		public Image GuestInterestIcon;
		public Text guestInterestText;

		public TimerView GuestOpinionBar;
		public Text GuestOpinionText;

		public PartyArtLibrary ArtConfig;

		private GuestVO _guest;
		private GuestSprite _sprite;
		private GameObject _guestAvatar;
		private RemarkVO _remark;
		private bool _isIntoxicated=false;
		private bool _isTargeted;
		private PartyArtLibrary _library; 

		void Awake()
		{
			AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Subscribe<GuestVO []>(PartyMessages.GUESTS_TARGETED, HandleTargeted);
			AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
			AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
			GuestOpinionBar.Subscribe(HandleInterestTimer);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Unsubscribe<GuestVO []>(PartyMessages.GUESTS_TARGETED, HandleTargeted);
			AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
			AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);
			GuestOpinionBar.Unsubscribe(HandleInterestTimer);
		}


		private void HandleInterestTimer(TimerView timer)
		{
			if (!_guest.IsLockedIn && timer.Complete)
			{
				_guest.State = GuestState.Bored;
			}
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
				EnemyVO enemy = _guest as EnemyVO;
				bool isEnemy = (enemy != null);
				string phrase = isEnemy ? "enemy_" : "guest_";

				GuestNameText.text = _guest.Name;
				GuestNameText.color = isEnemy ? Color.red : Color.white;

				GuestImage.sprite = _sprite.GetSprite(_isIntoxicated ? 50 : _guest.Opinion);

				GuestOpinionBar.gameObject.SetActive(!_guest.IsLockedIn);
				GuestOpinionBar.Percent = 0.1f*(float)_guest.Opinion;
				if (_guest.Opinion <= 0) _guest.State = GuestState.Bored;

            	UpdateColor();

            	if (isEnemy && enemy.attackNumber >= 0)
            	{
            		phrase += "attack." + enemy.attackNumber.ToString();
            	}
            	else
            	{
            		phrase += _guest.State.ToString();
            	}
				GuestOpinionText.text = AmbitionApp.GetModel<LocalizationModel>().GetString(phrase);
			}
		}

		private void HandleIntoxication(int tox)
		{
			_isIntoxicated = (tox >= 50);
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

			GuestImage.color = c;
		}
	}
}