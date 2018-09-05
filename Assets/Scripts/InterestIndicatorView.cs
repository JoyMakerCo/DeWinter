using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class InterestIndicatorView : MonoBehaviour
	{
		private const float FILL_SECONDS = 0.5f;
		private GuestVO _guest;
		private int _index;
        private Animator _animator;
        private RemarkVO _remark;

		public Image OpinionIndicator;		
		public Image InterestIcon;
        public Image InterestIconBorder;
		public Util.ColorConfig ColorConfig;
		public SpriteConfig InterestSprites;
		public SpriteConfig BorderSprites;
        public ParticleSystem PositiveEffect;
        public ParticleSystem NegativeEffect;
        public ParticleSystem NeutralEffect;

		void Awake()
		{
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
            AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleSelected);
			AmbitionApp.Subscribe<GuestVO>(HandleGuest);
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
			_index = transform.GetSiblingIndex();
            _animator = GetComponent<Animator>();
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);			
			AmbitionApp.Unsubscribe<GuestVO>(HandleGuest);
            AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleSelected);
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
		}

		private void HandleGuests(GuestVO [] guests)
		{

			bool enabled = guests != null && guests.Length > _index;
			_guest = enabled ? guests[_index] : null;
			gameObject.SetActive(enabled);
			if (enabled) HandleGuest(_guest);
		}

		private void HandleGuest(GuestVO guest)
		{
			if (guest == _guest)
			{
				StopAllCoroutines();
				StartCoroutine(FillMeter((_guest.Interest >=  _guest.MaxInterest) ? 1f : (float)_guest.Interest/((float)_guest.MaxInterest)));
				InterestIcon.sprite = InterestSprites.GetSprite(_guest.Like);
				InterestIconBorder.sprite = BorderSprites.GetSprite(_guest.State.ToString());
			}
		}


        private void HandleRemark(RemarkVO remark)
        {
            _remark = remark;
        }

        private void HandleSelected(GuestVO[] guests)
        {
            if (_animator != null
                && _guest != null
                && _remark != null
                && guests != null
                && Array.IndexOf(guests, _guest) >= 0)
            {
                if (_remark.Interest == _guest.Like)
                {
                    _animator.SetTrigger("Positive Remark");
                }
                else if (_remark.Interest == _guest.Dislike)
                {
                    _animator.SetTrigger("Negative Remark");
                }
                else
                {
                    _animator.SetTrigger("Neutral Remark");
                }
            }
        }


		IEnumerator FillMeter(float percent)
		{
			float startFill = OpinionIndicator.fillAmount;
			float fill;
			Util.ColorMap c0, c1;
			int colorindex;
			float delta = (percent-startFill) / FILL_SECONDS;
			float colorCount = ColorConfig.Colors.Length-1;
			for(float t=0f; t < FILL_SECONDS; t += Time.deltaTime)
			{
				fill = startFill + delta*t;
				OpinionIndicator.fillAmount = fill = (fill > 1f ? 1f : fill < 0f ? 0f : fill);
				fill*=colorCount;
				colorindex = Mathf.FloorToInt(fill);
				c0 = ColorConfig.Colors[colorindex];
				c1 = ColorConfig.Colors[Math.Min(colorindex+1, (int)colorCount-1)];
				OpinionIndicator.color = Color.Lerp(c0.Color, c1.Color, fill%1f);
				yield return null;
			}
			OpinionIndicator.fillAmount = percent;
		}

        public void TriggerNeutralParticles()
        {
            if (NeutralEffect != null)
                NeutralEffect.Play();
        }

        public void TriggerPositiveParticles()
        {
            if (PositiveEffect != null)
                PositiveEffect.Play();
        }

        public void TriggerNegativeParticles()
        {
            if (NegativeEffect != null)
                NegativeEffect.Play();
        }
	}
}
