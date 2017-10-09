using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public abstract class TextMessageView<T> : MonoBehaviour
	{
		public string Label;
		public string ValueID;

		private Text _text;
		public string Text
		{
			get { return _text.text; }
			set { _text.text = value; }
		}

		private MessageSvc _messages = App.Service<MessageSvc>();

		void Awake()
		{
			_text = GetComponent<Text>();
			_messages.Subscribe<T>(ValueID, HandleValue);
		}

		void OnDestroy()
		{
			_messages.Unsubscribe<T>(ValueID, HandleValue);
		}

		protected abstract void HandleValue(T value);
	}
}
