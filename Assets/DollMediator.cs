using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class DollMediator : MonoBehaviour
	{
		public SpriteConfig DressConfig;

		private Image _dollImage;

		void Awake()
		{
			_dollImage = GetComponent<Image>();
		}

		void OnEnable()
		{
			AmbitionApp.Subscribe<OutfitVO>(HandleOutfit);
		}

		void OnDisable()
		{
			AmbitionApp.Unsubscribe<OutfitVO>(HandleOutfit);
		}

		private void HandleOutfit(OutfitVO outfit)
		{
			Sprite s = (outfit != null)
				? DressConfig.GetSprite(outfit.Style)
				: null;
			_dollImage.sprite = (s != null)
				? s
				: DressConfig.Sprites[0].Sprite;
		}
	}
}
