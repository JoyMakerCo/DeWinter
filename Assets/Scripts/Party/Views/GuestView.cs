using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ambition
{
	public class GuestView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		private const float FILL_SECONDS = 0.5f;
		private readonly static string[] POSES = new string[]{"putout", "disapproval", "neutral", "approval", "charmed"};

		public int Index;

		public Image OpinionIndicator;
        public Color MinInterestColor;
        public Color MidInterestColor;
        public Color MaxInterestColor;
		public Image InterestIcon;
        public Image InterestIconBorder;
        public AvatarCollection Avatars;
		public SpriteConfig Interests;
        public Animator Animator;

		private GuestVO _guest;
		public GuestVO Guest
		{
			get { return _guest; }
		}

		private AvatarVO _avatar;
		private RemarkVO _remark;
		private Image _image;
		private bool _isIntoxicated=false;
		private string _pose;

		void Awake()
		{
			_image = GetComponent<Image>();
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
		}

		void OnEnable()
		{
            AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleTargeted);
            AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleSelected);
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
		}

		void OnDisable()
	    {
            AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleTargeted);
            AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleSelected);
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			StopAllCoroutines();
	    }

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<GuestVO []>(HandleGuests);
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
                InterestIconBorder.enabled = setEnabled;
			}

			if (setEnabled)
			{
				EnemyVO enemy = _guest as EnemyVO;
				bool isEnemy = (enemy != null);

				StartCoroutine(FillMeter((_guest.Interest >=  _guest.MaxInterest) ? 1f : (float)_guest.Interest/((float)_guest.MaxInterest)));

				if (_avatar.ID == null || _avatar.ID != _guest.Avatar)
				{
					if (_guest.Avatar != null) _avatar = Avatars.GetAvatar(_guest.Avatar);
					else
					{
						AvatarVO[] avatars = Avatars.Find(_guest.Gender, "party");
						_avatar = avatars[Util.RNG.Generate(avatars.Length)];
						_guest.Avatar = _avatar.ID;
					}
					_guest.Gender = _avatar.Gender;
				}

                _pose = _isIntoxicated
					? "neutral"
					: _guest.Interest <= 0
					? "bored"
					: POSES[(int)(_guest.State)];
				_image.sprite = _avatar.GetPose(_pose);

				if (_image.sprite == null)
					_image.sprite = _avatar.GetPose("neutral");

				InterestIcon.sprite = Interests.GetSprite(_guest.Like);
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
		}

		private void HandleSelected(GuestVO [] guests)
		{
            string pose = _pose;
            if (_guest != null
				&& _remark != null
				&& guests != null
				&& Array.IndexOf(guests, Guest) >= 0)
			{
				if (_remark.Interest == Guest.Like)
				{
                    //pose = POSES[POSES.Length - 1];
                    //_image.sprite = _avatar.GetPose(pose);
                    Animator.SetTrigger("Positive Remark");
				}
				else if (_remark.Interest == Guest.Dislike)
				{
                    //pose = "disapproval";
                    //_image.sprite = _avatar.GetPose(pose);
                    Animator.SetTrigger("Negative Remark");
				}
				else
				{
                    //pose = "approval";
                    //_image.sprite = _avatar.GetPose(pose);
                    Animator.SetTrigger("Neutral Remark");
				}
            }
		}

		private void HandleTargeted(GuestVO [] guests)
		{
			string pose = _pose;
			if (!_isIntoxicated
				&& _remark != null
				&& _guest != null
				&& guests != null
				&& Array.IndexOf(guests, Guest) >= 0)
			{
				if (_remark.Interest == Guest.Like)
				{
					pose = POSES[POSES.Length-1];
				}
				else if (_remark.Interest == Guest.Dislike)
				{
					pose = "disapproval";
				}
				else
				{
					pose = "approval";
				}
			}
			_image.sprite = _avatar.GetPose(pose);
		}

		System.Collections.IEnumerator FillMeter(float percent)
		{
			float t = 0;
			float startFill = OpinionIndicator.fillAmount;
			while (t<FILL_SECONDS)
			{
				OpinionIndicator.fillAmount = startFill + ((percent-startFill) * t / FILL_SECONDS);
				t += Time.deltaTime;
                //This is the only way to to a three part Lerps function with colors
                if (OpinionIndicator.fillAmount < 0.5f)
                {
                    OpinionIndicator.color = Color.Lerp(MinInterestColor, MidInterestColor, OpinionIndicator.fillAmount * 2);
                }
                else
                {
                    OpinionIndicator.color = Color.Lerp(MidInterestColor, MaxInterestColor, (OpinionIndicator.fillAmount-0.5f) * 2);
                }
				yield return null;
			}
			OpinionIndicator.fillAmount = percent;
		}
	}
}
