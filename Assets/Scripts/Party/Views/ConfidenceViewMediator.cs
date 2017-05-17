using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ConfidenceViewMediator : MonoBehaviour
	{
		private int _maxConfidence;
		private Text _text;
		private Scrollbar _bar;

		void Start()
		{
			_bar = gameObject.GetComponentInChildren<Scrollbar>();
			_text = gameObject.GetComponentInChildren<Text>();
			_maxConfidence = AmbitionApp.GetModel<PartyModel>().MaxConfidence;
			AmbitionApp.Subscribe<int>(GameConsts.CONFIDENCE, HandleConfidence);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(GameConsts.CONFIDENCE, HandleConfidence);
		}

		private void HandleConfidence(int confidence)
		{
			_text.text = "Confidence: " + confidence.ToString() + " / " + _maxConfidence.ToString();
			_text.color = (confidence <= 25) ? Color.red : Color.white;
			_bar.value = (float)confidence/(float)_maxConfidence;
		}
	}
}