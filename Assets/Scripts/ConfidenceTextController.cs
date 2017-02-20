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
	        DeWinterApp.Subscribe<AdjustBalanceVO>(HandleConfidenceUpdate);
	    }

		private void HandleConfidenceUpdate(AdjustBalanceVO vo)
	    {
	    	if (!vo.IsRequest)
	    	{
		        myText.text = "Confidence: " + _model.Party.currentPlayerConfidence + "/" + _model.Party.maxPlayerConfidence;
		    }
	    }
	}
}