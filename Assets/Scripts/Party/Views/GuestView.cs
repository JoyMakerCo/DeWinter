using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ambition
{
    public class GuestView : GuestViewMediator, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
        public Animator Animator;

        private AvatarView _avatar;
		private RemarkVO _remark;
		private bool _isIntoxicated=false;
        private string _pose;

        private void Awake()
        {
            _avatar = GetComponent<AvatarView>();
            InitGuest();
        }

        void OnEnable()
		{
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleTargeted);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
		}

        void OnDisable()
	    {
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleTargeted);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelected);
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
			AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
			StopAllCoroutines();
	    }

        private void OnDestroy()
        {
            Cleanup();
        }

        protected override void HandleGuest(GuestVO guest)
	    {
            if (guest != null && guest == Guest)
            {
                // Only do this work if the state is changing.
                if (_avatar.ID == null || _avatar.ID != Guest.Avatar)
                {
                    if (Guest.Avatar != null) _avatar.ID = Guest.Avatar;
                    else
                    {
                        AvatarVO[] avatars = _avatar.Collection.Find(Guest.Gender, "party");
                        _avatar.ID = Guest.Avatar = Util.RNG.TakeRandom(avatars).ID;
                    }
                    Guest.Gender = _avatar.Avatar.Gender;
                }

                if (guest.State != GuestState.Offended)
                {
                    if (_isIntoxicated) _pose = "neutral";
                    else switch (guest.State)
                        {
                            case GuestState.Bored:
                                _pose = "bored";
                                break;
                            case GuestState.Charmed:
                                _pose = "charmed";
                                break;
                            case GuestState.Interested:
                                _pose = "approval";
                                break;
                            default:
                                _pose = "neutral";
                                break;
                        }
                    if (_pose != _avatar.Pose)
                        _avatar.Pose = _pose;
                    if (_avatar.Sprite == null)
                        _avatar.Pose = _pose = "neutral";
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
	    }

		private void HandleIntoxication(int tox)
		{
			_isIntoxicated = (tox >= 50);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
            AmbitionApp.SendMessage<GuestVO>(PartyMessages.TARGET_GUEST, Guest);
		}

		public void OnPointerExit(PointerEventData eventData)
	    {
	    	AmbitionApp.SendMessage<GuestVO>(PartyMessages.TARGET_GUEST, null);
	    }

		public void OnPointerClick(PointerEventData eventData)
	    {
            AmbitionApp.SendMessage(PartyMessages.SELECT_GUEST, Guest);
        }

		private void HandleRemark(RemarkVO remark)
		{
			_remark = remark;
		}

        private void HandleSelected(GuestVO guest)
		{
            if (_remark != null && guest != null && Guest == guest)
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

        private void HandleTargeted(GuestVO guest)
		{
            if (guest == null) _avatar.Pose = _pose;
            else if (!_isIntoxicated && _remark != null && Guest == guest)
            {
                if (_remark.Interest == Guest.Like)
                {
                    _avatar.Pose  = "charmed";
                }
                else if (_remark.Interest == Guest.Dislike)
                {
                    _avatar.Pose = "putout";
                }
                else
                {
                    _avatar.Pose = "approval";
                }
            }
		}
	}
}
