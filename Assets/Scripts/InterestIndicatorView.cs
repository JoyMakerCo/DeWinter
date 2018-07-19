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

		public Image OpinionIndicator;		
		public Image InterestIcon;
        public Image InterestIconBorder;
		public Util.ColorConfig ColorConfig;
		public SpriteConfig InterestSprites;
		public SpriteConfig BorderSprites;
		void Awake()
		{
			AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
			AmbitionApp.Subscribe<GuestVO>(HandleGuest);
			_index = transform.GetSiblingIndex();

		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);			
			AmbitionApp.Unsubscribe<GuestVO>(HandleGuest);
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
	}
}
