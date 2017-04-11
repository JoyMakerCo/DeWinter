using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class IntValueListener : MonoBehaviour
	{
		public string Value;
		public string Label;
		private Text _text;

		void Start ()
		{
			_text = GetComponent<Text>();
			DeWinterApp.Subscribe<int>(Value, HandleValue);
		}

		void OnDestroy()
		{
			DeWinterApp.Unsubscribe<int>(Value, HandleValue);
		}

		private void HandleValue(int value)
		{
			if (!string.IsNullOrEmpty(Label))
				_text = Label + " " + value.ToString("###,###");
			else
				_text = value.ToString("###,###");
		}
	}
}