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
		private int _guestIndex;

		public Image OpinionIndicator;		
		public Image InterestIcon;
        public Image InterestIconBorder;
		public Util.ColorConfig ColorConfig;
		public SpriteConfig InterestSprites;
		public SpriteConfig BorderSprites;
		void Awake()
		{
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
			_guestIndex = transform.GetSiblingIndex();
		}

		void OnDestroy()
		{
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);			
		}

		private void HandleGuests(GuestVO [] guests)
		{
			bool enabled = guests != null && guests.Length > _guestIndex;
			GuestVO guest = enabled ? guests[_guestIndex] : null;
			gameObject.SetActive(enabled);
			if (enabled)
			{
				StopAllCoroutines();
				StartCoroutine(FillMeter((guest.Interest >=  guest.MaxInterest) ? 1f : (float)guest.Interest/((float)guest.MaxInterest)));
				InterestIcon.sprite = InterestSprites.GetSprite(guest.Like);
				InterestIconBorder.sprite = BorderSprites.GetSprite(guest.State.ToString());
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
				OpinionIndicator.fillAmount = fill;
				if (fill < 1f)
				{
					fill*=colorCount;
					colorindex = Mathf.FloorToInt(fill);
					c0 = ColorConfig.Colors[colorindex];
					c1 = ColorConfig.Colors[colorindex+1];
					OpinionIndicator.color = Color.Lerp(c0.Color, c1.Color, fill%1f);
				}
				yield return null;
			}
			OpinionIndicator.fillAmount = percent;
		}
	}
}
