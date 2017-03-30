using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class GeneralReputationBenefitsListController : MonoBehaviour
	{
		private Text _text;
	    void Start()
	    {
	    	
	        _text = this.GetComponent<Text>();
			DeWinterApp.Subscribe<PlayerReputationVO>(HandleReputation);
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<PlayerReputationVO>(HandleReputation);
	    }

	    private void HandleReputation(PlayerReputationVO rep)
	    {
	    	GameModel model = DeWinterApp.GetModel<GameModel>();
			_text.text = "Reputation Level Benefits\n" + model.BenefitsList;
	    }
	}
}