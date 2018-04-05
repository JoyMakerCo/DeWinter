using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class LocalizedText : MonoBehaviour
{
	private Text _text;
	private string _phrase;

	void Awake ()
	{
		_text = GetComponent<Text>();
		Debug.Assert(_text != null, "No text field found on GameObject \"" + this.gameObject.name + "\"");
		Phrase = _text.text;
	}

	public string Phrase
	{
		get { return _phrase; }
		set {
			string str = App.Service<ModelSvc>().GetModel<LocalizationModel>().GetString(value);
			_phrase = value;
			_text.text = str == null ? _phrase : str;
		}
	}
}
