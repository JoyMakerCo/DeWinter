using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ambition
{
	public class ConfidenceTextController : MonoBehaviour
	{
	    Text myText;
	    private PartyModel _model;

	    // Use this for initialization
	    void Start()
	    {
	        myText = this.GetComponent<Text>();
			_model = AmbitionApp.GetModel<PartyModel>();
			AmbitionApp.Subscribe<AdjustValueVO>(HandleConfidenceUpdate);
	    }

		private void HandleConfidenceUpdate(AdjustValueVO vo)
	    {
	    	if (!vo.IsRequest)
	    	{
		        myText.text = "Confidence: " + _model.Party.currentPlayerConfidence + "/" + _model.Party.maxPlayerConfidence;
		    }
	    }
	}
}