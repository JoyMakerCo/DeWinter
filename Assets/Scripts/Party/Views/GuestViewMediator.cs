using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	[Serializable]
	public class GuestViewMediator : MonoBehaviour
	{
		[HideInInspector]
		public int Index;

		[HideInInspector]
		public PartyArtLibrary ArtLibrary; 

		public Image GuestImage;
		public GameObject OpinionIndicator;
		public Image InterestIndicator;
		public Text NameText;

		private GuestVO _guest;
		private GuestSprite _sprite;
		private GameObject _guestAvatar;
		private RemarkVO _remark;
		private bool _isIntoxicated=false;
		private bool _isTargeted;

		void Start()
		{
			AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Subscribe<GuestVO []>(PartyMessages.GUESTS_TARGETED, HandleTargeted);
			AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
			AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Unsubscribe<GuestVO []>(PartyMessages.GUESTS_TARGETED, HandleTargeted);
			AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
			AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);
		}

	    void OnMouseOver()
	    {
	    	if (_remark != null)
	    	{
	    		AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_TARGETED, _guest);
	    	}
	    }

		void OnMouseOut()
	    {
    		AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_TARGETED, null);
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
			// Is there a guest? Hide/Show art
			bool isActive = guests.Length > Index;
			_guest = isActive ? guests[Index] : null;
			GuestImage.enabled = isActive;
			OpinionIndicator.SetActive(isActive);
			InterestIndicator.enabled = isActive;
			NameText.enabled = isActive;

			// Update the art to show the current guest state`
			if (isActive)
			{
				EnemyVO enemy = _guest as EnemyVO;
				bool isEnemy = (enemy != null);
				string phrase = isEnemy ? "enemy_" : "guest_";
				GuestSprite [] sprites = _guest.IsFemale
					? ArtLibrary.FemaleGuestSprites
					: ArtLibrary.MaleGuestSprites;

				if (_guest.Variant < 0)
				{
					_guest.Variant = new System.Random().Next(sprites.Length);
				}

				_sprite = sprites[_guest.Variant];

				NameText.text = _guest.Name;
				NameText.color = isEnemy ? Color.red : Color.white;

				OpinionIndicator.gameObject.SetActive(!_guest.IsLockedIn);
// TODO: Set percent??
//				OpinionIndicator.Percent = 0.1f*(float)_guest.Opinion;
				if (_guest.Opinion <= 0) _guest.State = GuestState.Bored;

				GuestImage.sprite = _sprite.GetSprite(_isIntoxicated ? GuestState.Interested : _guest.State);

            	UpdateColor();

            	if (isEnemy && enemy.attackNumber >= 0)
            	{
            		phrase += "attack." + enemy.attackNumber.ToString();
            	}
            	else
            	{
            		phrase += _guest.State.ToString();
            	}
            	// TODO: Opinion artwork goes here
//				OpinionText.text = AmbitionApp.GetModel<LocalizationModel>().GetString(phrase);
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
			else if (_remark.Interest == _guest.Like) c = Color.green;
			else if (_remark.Interest == _guest.Disike) c = Color.red;
			else c = Color.white;

			GuestImage.color = c;
		}
	}
}