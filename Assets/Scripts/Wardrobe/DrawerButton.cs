﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	[Serializable]
	public struct RibbonMapping
	{
		public string style;
		public Sprite sprite;
	}

	public class DrawerButton : MonoBehaviour
	{
		public RibbonMapping[] Styles;

		public Text ItemName;
		public GameObject Note;
		public Image Ribbon;

		private Button _button;

		void Awake()
		{
			_button = GetComponent<Button>();
		}

		public void SetItem(ItemVO item)
		{
			bool enabled = (item != null);
			_button.enabled = enabled;
			ItemName.enabled = enabled;
			Ribbon.enabled = enabled;
			Note.SetActive(enabled);
			if (enabled)
			{
				ItemName.text = item.Name;
				if (item.State.ContainsKey(ItemConsts.STYLE))
				{
					RibbonMapping mapping = Array.Find(Styles, s=> s.style == (string)(item.State[ItemConsts.STYLE]));
					Ribbon.sprite = mapping.sprite;
				}
				else Ribbon.sprite = null;
				Ribbon.enabled = (Ribbon.sprite != null);
				Note.SetActive(item.State.ContainsKey(ItemConsts.GIFT));
			}
		}
	}
}