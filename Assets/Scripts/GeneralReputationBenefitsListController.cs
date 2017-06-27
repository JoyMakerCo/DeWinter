using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class GeneralReputationBenefitsListController : MonoBehaviour
	{
		private Text _text;
	    void Start()
	    {
	    	
	        _text = this.GetComponent<Text>();
			AmbitionApp.Subscribe<PlayerReputationVO>(HandleReputation);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<PlayerReputationVO>(HandleReputation);
	    }

	    private void HandleReputation(PlayerReputationVO rep)
	    {
	    	GameModel model = AmbitionApp.GetModel<GameModel>();
			_text.text = "Reputation Level Benefits\n" + model.BenefitsList;
	    }
	}
}