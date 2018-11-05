using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class InterestIndicatorView : GuestViewMediator
	{
		private const float FILL_SECONDS = 0.5f;
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

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            InitGuest();
        }

        private void OnDestroy()
        {
            Cleanup();
        }
        
        private void OnEnable()
        {
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelect);
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
        }

        private void OnDisable()
        {
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleSelect);
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
        }

        override protected void HandleGuest(GuestVO guest)
		{
            if (guest != null && guest == _guest)
			{
                if (guest.State != GuestState.Offended)
                {
                    float amount = (_guest.Opinion >= 100) ? 1f : ((float)_guest.Opinion) * .01f;
                    if (gameObject.activeInHierarchy)
                    {
                        StopAllCoroutines();
                        StartCoroutine(FillMeter(amount));
                    }
                    else
                    {
                        OpinionIndicator.fillAmount = amount;
                    }
                    InterestIcon.sprite = InterestSprites.GetSprite(_guest.Like);
                    InterestIconBorder.sprite = BorderSprites.GetSprite(_guest.State.ToString());
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
		}

        private void HandleRemark(RemarkVO remark)
        {
            _remark = remark;
        }

        private void HandleSelect(GuestVO guest)
        {
            if (_remark != null && guest != null && guest == _guest)
            {
                if (_remark.Interest == _guest.Like)
                {
                    if (_animator != null)
                        _animator.SetTrigger("Positive Remark");
                    if (PositiveEffect != null)
                        PositiveEffect.Play();
                }
                else if (_remark.Interest == _guest.Dislike)
                {
                    if (_animator != null)
                        _animator.SetTrigger("Negative Remark");
                    if (NegativeEffect != null)
                        NegativeEffect.Play();
                }
                else
                {
                    if (_animator != null)
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
