using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class SortButton : MonoBehaviour
	{
		public bool Ascending=false;
		public Image SortIcon;
		public string SortOn;

		private Button _button;

		public Button.ButtonClickedEvent onClick
		{
			get {
				InitButton();
				return _button.onClick;
			}
		}

		void Awake()
		{
			InitButton();
		}

		private void InitButton()
		{
			if (_button == null)
			{
				_button = GetComponent<Button>();
				_button.onClick.AddListener(HandleClick);
			}
		}

		void OnDestroy()
		{
			_button.onClick.RemoveListener(HandleClick);
		}

		private void HandleClick()
		{
			Ascending = !Ascending;
			SortIcon.transform.localScale = new Vector3(1f, Ascending ? 1f : -1f, 1f);
		}
	}
}
