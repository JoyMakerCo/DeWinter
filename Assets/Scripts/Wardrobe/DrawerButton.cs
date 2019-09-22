using System;
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

		private ItemVO _item;
		private Button _button;

		void Awake()
		{
			_button = GetComponent<Button>();
		}

		void OnEnable()
		{
			_button.onClick.AddListener(HandleClick);
		}

		void OnDisable()
		{
			_button.onClick.RemoveListener(HandleClick);
		}

		public void SetItem(ItemVO item)
		{
            bool drawerEnabled = ((_item = item) != null);
			_button.enabled = drawerEnabled;
			ItemName.enabled = drawerEnabled;
			Ribbon.enabled = drawerEnabled;
			Note.SetActive(drawerEnabled);
			if (drawerEnabled)
			{
                string style = OutfitWrapperVO.GetStyle(item);
				ItemName.text = item.Name;
                RibbonMapping mapping = Array.Find(Styles, s => s.style == style);
                Ribbon.sprite = mapping.sprite;
				Ribbon.enabled = (Ribbon.sprite != null);
                Note.SetActive(item.State?.ContainsKey(ItemConsts.GIFT) ?? false);
			}
		}

		private void HandleClick()
		{
			transform.SetAsLastSibling();
			if (_item != null) AmbitionApp.SendMessage(InventoryMessages.EQUIP, _item);
		}
	}
}
