using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public abstract class TextMessageView<T> : MonoBehaviour
	{
		public string ValueID;
		private Text _text;

		public TextMessageView(string valueID)
		{
			ValueID = valueID;
		}

		public string Text
		{
			get { return (_text != null) ? _text.text : null; }
			set { if (_text != null) _text.text = value; }
		}

		void Awake()
		{
			_text = GetComponent<Text>();
			AmbitionApp.Subscribe<T>(ValueID, HandleValue);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<T>(ValueID, HandleValue);
		}

		protected virtual void InitValue() {} 
		protected abstract void HandleValue(T value);
	}
}
