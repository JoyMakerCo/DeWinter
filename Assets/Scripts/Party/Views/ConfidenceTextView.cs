using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ConfidenceTextView : MonoBehaviour
	{
		private PartyModel _model;
		private Text _text;
		void Awake()
		{
			_model = AmbitionApp.GetModel<PartyModel>();
			_text = GetComponent<Text>();
			AmbitionApp.Subscribe<int>(GameConsts.CONFIDENCE, HandleValue);
		}
		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(GameConsts.CONFIDENCE, HandleValue);
		}

		void OnEnable()
		{
			HandleValue(_model.Confidence);
		}

		protected void HandleValue (int value)
		{

			_text.text = "Confidence: " + value.ToString("N0") + (_model != null ? ("/" + _model.MaxConfidence.ToString("N0")) : "");
		}
	}
}
