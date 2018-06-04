using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ConfidenceTextView : MonoBehaviour
	{
		public Text ConfidenceTxt;
		public Text ConfidenceMaxTxt;
		void Awake()
		{
			AmbitionApp.Subscribe<int>(GameConsts.CONFIDENCE, HandleValue);
		}
		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(GameConsts.CONFIDENCE, HandleValue);
		}

		void OnEnable()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			ConfidenceMaxTxt.text = model.MaxConfidence.ToString();
			HandleValue(model.Confidence);
		}

		protected void HandleValue (int value)
		{
			ConfidenceTxt.text = value.ToString();
		}
	}
}
