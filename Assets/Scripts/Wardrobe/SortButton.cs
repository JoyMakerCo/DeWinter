using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class SortButton : MonoBehaviour
	{
		public bool Ascending=false;
		public Sprite AscendingIcon;
		public Sprite DescendingIcon;
		public string SortOn;

		private Image _icon;
		private Button _button;

		public Button.ButtonClickedEvent onClick
		{
			get { return _button.onClick; }
		}

		void Awake()
		{
			_icon = GetComponent<Image>();
			_button = GetComponent<Button>();
			_button.onClick.AddListener(HandleClick);
		}

		void OnDestroy()
		{
			_button.onClick.RemoveListener(HandleClick);
		}

		private void HandleClick()
		{
			Ascending = !Ascending;
			_icon.sprite = Ascending ? AscendingIcon : DescendingIcon;
		}
	}
}
