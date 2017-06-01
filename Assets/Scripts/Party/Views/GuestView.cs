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
		public Image GuestImage;
		public Scrollbar OpinionIndicator;
		public Image InterestIcon;
		public Text NameText;
	}
}
