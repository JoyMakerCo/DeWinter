using System;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class DialogButtonHandler : MonoBehaviour
	{
		public Dialog.DialogCanvasManager canvas;
		public GameObject DialogPrefab;

		protected Button _button;

		void Start()
		{
			_button = gameObject.GetComponent<Button>();
			if (_button != null)
				_button.onClick.AddListener(OnClick);
		}

		protected void OnClick()
		{
			canvas.Open(DialogPrefab);
		}
	}
}
