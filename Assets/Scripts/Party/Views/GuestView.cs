using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ambition
{
	public class GuestView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		public int Index;

		public Image OpinionIndicator;
		public Image InterestIcon;
		public Text NameText;
		public Core.ICommand mb;

		public GuestConfig GuestArtConfig;

		private GuestVO _guest;
		private RemarkVO _remark;
		private Image _image;
		private bool _isIntoxicated=false;

		void Awake()
		{
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
			AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleTargets);
			AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			_image = GetComponent<Image>();
		}

		void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<GuestVO []>(HandleGuests);
			AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleTargets);
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
	    }

	    private void HandleGuests(GuestVO[] guests)
	    {
	    	_guest = (guests.Length > Index) ? guests[Index] : null;

	    	// Shows/hides components of a guest view based on the data
			bool setEnabled = (_guest != null);
			// Only do this work if the state is changing.
	    	if (setEnabled != _image.enabled)
	    	{
				this.gameObject.SetActive(setEnabled);
				_image.enabled = setEnabled;
				OpinionIndicator.enabled = setEnabled;
				InterestIcon.enabled = setEnabled;
				NameText.enabled = setEnabled;
			}

			if (setEnabled)
			{
				EnemyVO enemy = _guest as EnemyVO;
				bool isEnemy = (enemy != null);

				NameText.text = _guest.Name;

				OpinionIndicator.gameObject.SetActive(!_guest.IsLockedIn);
				OpinionIndicator.fillAmount = (float)_guest.Interest/((float)_guest.MaxInterest);

				if (_guest.Variant < 0)
				{
					GuestSprite [] sprites = Array.FindAll(GuestArtConfig.GuestSprites, i=>i.IsFemale == _guest.IsFemale);
					int index = (new System.Random()).Next(sprites.Length);
					_guest.Variant = Array.IndexOf(GuestArtConfig.GuestSprites, sprites[index]);
				}

				_image.sprite = !_isIntoxicated
					? GuestArtConfig.GuestSprites[_guest.Variant].GetSprite(_guest)
					: GuestArtConfig.GuestSprites[_guest.Variant].BoredSprite;

				InterestMap interest = Array.Find(GuestArtConfig.InterestSprites, n=>n.Interest == _guest.Like);
				if (!default(InterestMap).Equals(interest))
					InterestIcon.sprite = interest.Sprite;
			}
	    }

		private void HandleIntoxication(int tox)
		{
			_isIntoxicated = (tox >= 50);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_TARGETED, _guest);
		}

		public void OnPointerExit(PointerEventData eventData)
	    {
	    	AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_TARGETED, null);
	    }

		public void OnPointerClick(PointerEventData eventData)
	    {
			AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_SELECTED, _guest);
	    }

		private void HandleRemark(RemarkVO remark)
		{
			_remark = remark;
			_image.color = Color.white;
		}

		private void HandleTargets(GuestVO[] guests)
		{
			if (_remark == null || guests == null || guests.Length == 0)
			{
				_image.color = Color.white;
			}
			else if (Array.IndexOf(guests, _guest) < 0)
			{
				_image.color = Color.gray;
			}
			else
			{
				if (_isIntoxicated) _image.color = Color.white;
				else if (_remark.Interest == _guest.Like) _image.color = Color.green;
				else if (_remark.Interest == _guest.Disike) _image.color = Color.red;
				else _image.color = Color.white;
			}
		}
	}
}
