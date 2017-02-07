using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class ReputationTracker : MonoBehaviour {
	    public Text numberText;
	    public Text toolTipText;

	    // Use this for initialization
	    void Start()
	    {
			DeWinterApp.Subscribe<PlayerReputationVO>(HandleReputationUpdate);
	    }

		private void HandleReputationUpdate(PlayerReputationVO repData)
	    {
			numberText.text = repData.ReputationLevel.ToString() + " (" + repData.Reputation.ToString("#,##0") + ")";                     
	    }
	}
}