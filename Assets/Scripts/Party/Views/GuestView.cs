using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	[Serializable]
	public struct GuestView
	{
		public GameObject GuestObject;
		public Image GuestImage;
		public Image OpinionIndicator;
		public Image InterestIcon;
		public Text NameText;

		public bool Enabled
		{
			get { return GuestImage.enabled; }
			set {
				GuestObject.SetActive(value);
				GuestImage.enabled = value;
				OpinionIndicator.enabled = value;
				InterestIcon.enabled = value;
				NameText.enabled = value;
			}
		}
	}
}
