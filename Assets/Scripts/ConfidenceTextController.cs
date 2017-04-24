using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class ConfidenceTextController : MonoBehaviour
	{
	    Text myText;
	    private PartyModel _model;

	    // Use this for initialization
	    void Start()
	    {
	        myText = this.GetComponent<Text>();
			_model = DeWinterApp.GetModel<PartyModel>();
			DeWinterApp.Subscribe<int>(GameConsts.CONFIDENCE, HandleConfidenceUpdate);
	    }

		private void HandleConfidenceUpdate(int confidence)
	    {
	        myText.text = "Confidence: " + confidence.ToString() + "/" + _model.Party.maxPlayerConfidence.ToString();
	    }
	}
}