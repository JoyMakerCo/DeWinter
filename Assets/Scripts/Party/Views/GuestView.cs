using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ambition
{
	public class GuestView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		private readonly static string[] POSES = new string[]{"putout", "neutral", "approval", "charmed"};
        public Animator Animator;

        private GuestVO _guest;
        public GuestVO Guest
        {
            get { return _guest; }
        }

        private AvatarView _avatar;
		private RemarkVO _remark;
		private bool _isIntoxicated=false;
        private string _pose;
        
		void Awake()
		{
            _avatar = GetComponent<AvatarView>();
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
            int index = transform.GetSiblingIndex();
	    	_guest = (index < guests.Length) ? guests[index] : null;

	    	// Shows/hides components of a guest view based on the data
			this.gameObject.SetActive(_guest != null);

			// Only do this work if the state is changing.
			if (this.gameObject.activeSelf)
			{
                if (_avatar.ID == null || _avatar.ID != _guest.Avatar)
				{
                    if (_guest.Avatar != null) _avatar.ID = _guest.Avatar;
					else
					{
						AvatarVO[] avatars = _avatar.Collection.Find(_guest.Gender, "party");
                        _avatar.ID = _guest.Avatar = Util.RNG.TakeRandom(avatars).ID;
					}
                    _guest.Gender = _avatar.Avatar.Gender;
				}

                _avatar.Pose = _pose = _isIntoxicated
					? "neutral"
					: _guest.Interest <= 0
					? "bored"
					: POSES[(int)(_guest.State)];
					
                if (_avatar.Sprite == null)
                    _avatar.Pose = _pose = "neutral";
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
			if (_guest != null
				&& _remark != null
				&& guests != null
				&& Array.IndexOf(guests, Guest) >= 0)
			{
				if (_remark.Interest == Guest.Like)
				{
					Animator.SetTrigger("Positive Remark");
				}
				else if (_remark.Interest == Guest.Dislike)
				{
                    Animator.SetTrigger("Negative Remark");
				}
				else
				{
                    Animator.SetTrigger("Neutral Remark");
				}
			}
		}

		private void HandleTargeted(GuestVO [] guests)
		{
            if (!_isIntoxicated
                && _remark != null
                && _guest != null
                && guests != null
                && Array.IndexOf(guests, Guest) >= 0)
            {
                if (_remark.Interest == Guest.Like)
                {
                    _avatar.Pose  = POSES[POSES.Length - 1];
                }
                else if (_remark.Interest == Guest.Dislike)
                {
                    _avatar.Pose = POSES[0];
                }
                else
                {
                    _avatar.Pose = "approval";
                }
            }
            else _avatar.Pose = _pose;
		}
	}
}
